using System.Net;
using System.Net.Http.Json;
using Domain.Common;
using Domain.Dynamics365;
using Xunit;

namespace Api.Tests;

public class Dynamics365EndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly Guid _testGuid = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public Dynamics365EndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("ms")]
    [InlineData("pg")]
    public async Task GetAccounts_ValidProvider_ReturnsOk(string provider)
    {
        var response = await _client.GetAsync($"/api/{provider}/v1/dynamics365/accounts?page=1&pageSize=20");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<Account>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        Assert.Equal(_testGuid, result.Items[0].AccountId);
    }

    [Theory]
    [InlineData("ms")]
    [InlineData("pg")]
    public async Task GetAccountById_ExistingGuid_ReturnsOk(string provider)
    {
        var response = await _client.GetAsync($"/api/{provider}/v1/dynamics365/accounts/{_testGuid}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var account = await response.Content.ReadFromJsonAsync<Account>();
        Assert.NotNull(account);
        Assert.Equal(_testGuid, account.AccountId);
        Assert.Equal("D365 Account", account.Name);
    }
}
