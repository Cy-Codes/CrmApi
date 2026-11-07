using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Domain.Common;
using Domain.Salesforce;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Api.Tests;

public sealed class AuthenticatedWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestSigningKey = "LocalStackCRMReferenceArchitectureJwtSigningSecretKey2026!";
    public const string TestIssuer = "https://auth.localstack.local";
    public const string TestAudience = "crm-api";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authentication:Enabled"] = "true",
                ["Authentication:Jwt:Authority"] = TestIssuer,
                ["Authentication:Jwt:Issuer"] = TestIssuer,
                ["Authentication:Jwt:Audience"] = TestAudience,
                ["Authentication:Jwt:SigningKey"] = TestSigningKey
            });
        });

        builder.ConfigureServices(services =>
        {
            var sfMock = new Mock<Application.Interfaces.ISalesforceQueryService>();
            sfMock.Setup(s => s.GetAccountsAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<Account>(
                    [new Account { Id = "001Test", Name = "Auth Corp", Industry = "Technology" }],
                    1,
                    QueryOptions.DefaultPage,
                    QueryOptions.DefaultPageSize));

            services.ReplaceWithSingletonMock(sfMock);
        });
    }

    public static string GenerateValidJwtToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TestSigningKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, "user-123"),
                new Claim(ClaimTypes.Role, "CrmReader"),
                new Claim("scope", "crm:read")
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = TestIssuer,
            Audience = TestAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public sealed class JwtAuthenticationTests : IClassFixture<AuthenticatedWebApplicationFactory>
{
    private readonly AuthenticatedWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public JwtAuthenticationTests(AuthenticatedWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RequestWithoutToken_WhenAuthEnabled_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/ms/v1/salesforce/accounts");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RequestWithValidJwtToken_WhenAuthEnabled_ReturnsOk()
    {
        var token = AuthenticatedWebApplicationFactory.GenerateValidJwtToken();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/ms/v1/salesforce/accounts");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HealthEndpoint_WhenAuthEnabled_AllowsAnonymousAccess()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
