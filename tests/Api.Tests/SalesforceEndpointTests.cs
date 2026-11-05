using System.Net;
using System.Net.Http.Json;
using Domain.Common;
using Domain.Salesforce;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Api.Tests;

public class SalesforceEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SalesforceEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("ms")]
    [InlineData("pg")]
    public async Task GetAccounts_ValidProvider_ReturnsOkWithPagedResult(string provider)
    {
        var response = await _client.GetAsync($"/api/{provider}/v1/salesforce/accounts?page=1&pageSize=20");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<Account>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("001000000000001AAA", result.Items[0].Id);
    }

    [Theory]
    [InlineData("ms")]
    [InlineData("pg")]
    public async Task GetAccountById_ExistingId_ReturnsOkWithAccount(string provider)
    {
        var response = await _client.GetAsync($"/api/{provider}/v1/salesforce/accounts/001000000000001AAA");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var account = await response.Content.ReadFromJsonAsync<Account>();
        Assert.NotNull(account);
        Assert.Equal("001000000000001AAA", account.Id);
        Assert.Equal("Acme Corp", account.Name);
    }

    [Fact]
    public async Task GetAccounts_InvalidProvider_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/invalid/v1/salesforce/accounts");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("ms")]
    [InlineData("pg")]
    public async Task GetContactsByAccountId_ReturnsOkList(string provider)
    {
        var response = await _client.GetAsync($"/api/{provider}/v1/salesforce/accounts/001000000000001AAA/contacts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contacts = await response.Content.ReadFromJsonAsync<List<Contact>>();
        Assert.NotNull(contacts);
        Assert.Single(contacts);
        Assert.Equal("Doe", contacts[0].LastName);
    }
}
