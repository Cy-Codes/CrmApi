using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SfAccount = Domain.Salesforce.Account;
using SfAccountWriteRequest = Domain.Salesforce.AccountWriteRequest;
using Task = System.Threading.Tasks.Task;

namespace Api.Tests;

public sealed class AuthenticatedWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestSigningKey = "LocalStackCRMReferenceArchitectureJwtSigningSecretKey2026!";
    public const string TestIssuer = "https://auth.localstack.local";
    public const string TestAudience = "crm-api";
    public const string ExistingAccountId = "001000000000001AAA";
    public const string MissingAccountId = "001000000000099AAA";
    public const long ExistingCompanyId = 101;
    public const long ConflictingCompanyId = 666;

    public Mock<ISalesforceQueryService> SalesforceQueryServiceMock { get; } = new();
    public Mock<ISalesforceCommandService> SalesforceCommandServiceMock { get; } = new();
    public Mock<IHubSpotCommandService> HubSpotCommandServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authentication:Jwt:Authority"] = TestIssuer,
                ["Authentication:Jwt:Issuer"] = TestIssuer,
                ["Authentication:Jwt:Audience"] = TestAudience,
                ["Authentication:Jwt:SigningKey"] = TestSigningKey
            });
        });

        builder.ConfigureServices(services =>
        {
            var existingAccount = new SfAccount { Id = ExistingAccountId, Name = "Auth Corp", Industry = "Technology" };

            SalesforceQueryServiceMock
                .Setup(s => s.GetAccountsAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<SfAccount>(
                    [existingAccount],
                    1,
                    QueryOptions.DefaultPage,
                    QueryOptions.DefaultPageSize));

            SalesforceCommandServiceMock
                .Setup(s => s.ExecuteCreateAccountAsync(It.IsAny<DatabaseProvider>(), It.IsAny<SfAccountWriteRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAccount);
            SalesforceCommandServiceMock
                .Setup(s => s.ExecuteUpdateAccountAsync(It.IsAny<DatabaseProvider>(), ExistingAccountId, It.IsAny<SfAccountWriteRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAccount);
            SalesforceCommandServiceMock
                .Setup(s => s.ExecuteUpdateAccountAsync(It.IsAny<DatabaseProvider>(), MissingAccountId, It.IsAny<SfAccountWriteRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SfAccount?)null);
            SalesforceCommandServiceMock
                .Setup(s => s.ExecuteDeleteAccountAsync(It.IsAny<DatabaseProvider>(), ExistingAccountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            SalesforceCommandServiceMock
                .Setup(s => s.ExecuteDeleteAccountAsync(It.IsAny<DatabaseProvider>(), MissingAccountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            HubSpotCommandServiceMock
                .Setup(s => s.ExecuteDeleteCompanyAsync(It.IsAny<DatabaseProvider>(), ConflictingCompanyId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ConflictException("Company is still referenced."));
            HubSpotCommandServiceMock
                .Setup(s => s.ExecuteCreateCompanyAsync(It.IsAny<DatabaseProvider>(), It.IsAny<CompanyWriteRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Company { CompanyId = ExistingCompanyId, Name = "HubSpot Co" });

            services
                .ReplaceWithSingletonMock(SalesforceQueryServiceMock)
                .ReplaceWithSingletonMock(SalesforceCommandServiceMock)
                .ReplaceWithSingletonMock(HubSpotCommandServiceMock);
        });
    }

    /// <summary>
    /// Pure helper minting an HS256 token carrying the given <c>roles</c> claims (none, one, or many).
    /// </summary>
    public static string GenerateJwtToken(
        IEnumerable<string>? roles = null,
        string signingKey = TestSigningKey,
        string audience = TestAudience)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(signingKey);
        var claims = (roles ?? [])
            .Select(static role => new Claim(CrmRoles.ClaimType, role))
            .Append(new Claim(JwtRegisteredClaimNames.Sub, "user-123"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = TestIssuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}

public sealed class JwtAuthenticationTests : IClassFixture<AuthenticatedWebApplicationFactory>
{
    private const string AccountsUrl = "/api/ms/v1/salesforce/accounts";
    private const string CompaniesUrl = "/api/pg/v1/hubspot/companies";
    private static readonly string[] AdminRoles = [CrmRoles.Admin];
    private static readonly string[] UserRoles = [CrmRoles.User];
    private static readonly string[] UnrelatedRoles = ["auditor"];

    private static readonly SfAccountWriteRequest ValidAccountRequest = new() { Name = "Auth Corp", OwnerId = "005000000000001AAA" };

    private readonly HttpClient _client;

    public JwtAuthenticationTests(AuthenticatedWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, string? token, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);
        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (body is not null)
        {
            request.Content = JsonContent.Create(body);
        }

        return _client.SendAsync(request);
    }

    private static string AccountUrl(string id) => $"{AccountsUrl}/{id}";

    [Fact]
    public async Task Get_WithoutToken_IsAnonymousReadOnlyAccess()
    {
        var response = await SendAsync(HttpMethod.Get, AccountsUrl, token: null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_WithValidToken_ReturnsOk()
    {
        var response = await SendAsync(HttpMethod.Get, AccountsUrl, AuthenticatedWebApplicationFactory.GenerateJwtToken());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HealthEndpoint_AllowsAnonymousAccess()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    public async Task Mutation_WithoutToken_ReturnsUnauthorized(string method)
    {
        var url = method == "POST" ? AccountsUrl : AccountUrl(AuthenticatedWebApplicationFactory.ExistingAccountId);
        var response = await SendAsync(new HttpMethod(method), url, token: null, ValidAccountRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Mutation_WithWrongSigningKey_ReturnsUnauthorized()
    {
        var forged = AuthenticatedWebApplicationFactory.GenerateJwtToken(AdminRoles, signingKey: "AnotherSecretKeyThatIsAtLeast32BytesLong!!");
        var response = await SendAsync(HttpMethod.Post, AccountsUrl, forged, ValidAccountRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Mutation_WithWrongAudience_ReturnsUnauthorized()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(AdminRoles, audience: "someone-else");
        var response = await SendAsync(HttpMethod.Post, AccountsUrl, token, ValidAccountRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    public async Task Mutation_WithTokenWithoutRoles_ReturnsForbidden(string method)
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken();
        var url = method == "POST" ? AccountsUrl : AccountUrl(AuthenticatedWebApplicationFactory.ExistingAccountId);
        var response = await SendAsync(new HttpMethod(method), url, token, ValidAccountRequest);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Mutation_WithUnrelatedRole_ReturnsForbidden()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(UnrelatedRoles);
        var response = await SendAsync(HttpMethod.Post, AccountsUrl, token, ValidAccountRequest);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Create_AsUser_ReturnsCreatedWithLocation()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(UserRoles);
        var response = await SendAsync(HttpMethod.Post, AccountsUrl, token, ValidAccountRequest);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.EndsWith(
            AccountUrl(AuthenticatedWebApplicationFactory.ExistingAccountId),
            response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task Create_AsAdmin_ReturnsCreated()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(AdminRoles);
        var response = await SendAsync(HttpMethod.Post, AccountsUrl, token, ValidAccountRequest);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidBody_ReturnsBadRequest()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(UserRoles);
        var response = await SendAsync(HttpMethod.Post, AccountsUrl, token, new { ownerId = "005000000000001AAA" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_AsUser_ReturnsOk()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(UserRoles);
        var response = await SendAsync(
            HttpMethod.Put,
            AccountUrl(AuthenticatedWebApplicationFactory.ExistingAccountId),
            token,
            ValidAccountRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Update_OfMissingEntity_ReturnsNotFound()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(AdminRoles);
        var response = await SendAsync(
            HttpMethod.Put,
            AccountUrl(AuthenticatedWebApplicationFactory.MissingAccountId),
            token,
            ValidAccountRequest);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_AsUser_ReturnsForbidden()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(UserRoles);
        var response = await SendAsync(HttpMethod.Delete, AccountUrl(AuthenticatedWebApplicationFactory.ExistingAccountId), token);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Delete_AsAdmin_ReturnsNoContent()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(AdminRoles);
        var response = await SendAsync(HttpMethod.Delete, AccountUrl(AuthenticatedWebApplicationFactory.ExistingAccountId), token);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_OfMissingEntity_ReturnsNotFound()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(AdminRoles);
        var response = await SendAsync(HttpMethod.Delete, AccountUrl(AuthenticatedWebApplicationFactory.MissingAccountId), token);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenRowIsReferenced_ReturnsConflictProblemDetails()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(AdminRoles);
        var response = await SendAsync(
            HttpMethod.Delete,
            $"{CompaniesUrl}/{AuthenticatedWebApplicationFactory.ConflictingCompanyId}",
            token);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Create_HubSpotCompany_AsUser_ReturnsCreated()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken(UserRoles);
        var response = await SendAsync(HttpMethod.Post, CompaniesUrl, token, new { name = "HubSpot Co" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.EndsWith($"{CompaniesUrl}/{AuthenticatedWebApplicationFactory.ExistingCompanyId}", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task Mutation_WithMultipleRoles_UsesHighestPrivilege()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateJwtToken([CrmRoles.User, CrmRoles.Admin]);
        var response = await SendAsync(HttpMethod.Delete, AccountUrl(AuthenticatedWebApplicationFactory.ExistingAccountId), token);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
