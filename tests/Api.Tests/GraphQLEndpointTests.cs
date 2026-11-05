using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Api.Tests;

public class GraphQLEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GraphQLEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostGraphQL_SalesforceAccountsQuery_ReturnsOkData()
    {
        var query = new
        {
            query = @"
                query {
                    salesforceAccounts(provider: MS, page: 1, pageSize: 5) {
                        id
                        name
                        industry
                    }
                }"
        };

        var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/graphql", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("salesforceAccounts", json);
        Assert.Contains("Acme Corp", json);
    }
}
