using System.Net;
using System.Net.Http.Json;
using Domain.Common;
using Domain.Salesforce;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Api.Tests;

public sealed class ExceptionHandlingAndFilterTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ExceptionHandlingAndFilterTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAccounts_WithFilters_PassesFiltersToQueryOptions()
    {
        _factory.SalesforceQueryServiceMock
            .Setup(s => s.GetAccountsAsync(
                DatabaseProvider.Ms,
                It.Is<QueryOptions>(o => o.Filters != null && o.Filters.ContainsKey("Industry") && o.Filters["Industry"] == "Technology"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Account>(
                [new Account { Id = "001Filtered", Name = "Tech Corp", Industry = "Technology" }],
                1,
                QueryOptions.DefaultPage,
                QueryOptions.DefaultPageSize));

        var response = await _client.GetAsync("/api/ms/v1/salesforce/accounts?Industry=Technology");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<Account>>();
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Tech Corp", result.Items[0].Name);
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsHealthyStatus()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("Healthy", json);
    }
}
