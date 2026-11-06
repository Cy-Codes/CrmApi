using System.Net;
using System.Net.Http.Json;
using Domain.Common;
using Domain.HubSpot;

namespace Api.Tests;

public sealed class HubSpotEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HubSpotEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("ms")]
    [InlineData("pg")]
    public async Task GetCompanies_ValidProvider_ReturnsOk(string provider)
    {
        var response = await _client.GetAsync($"/api/{provider}/v1/hubspot/companies?page=1&pageSize=20");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<Company>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        Assert.Equal(101, result.Items[0].CompanyId);
    }

    [Theory]
    [InlineData("ms")]
    [InlineData("pg")]
    public async Task GetCompanyById_ExistingId_ReturnsOk(string provider)
    {
        var response = await _client.GetAsync($"/api/{provider}/v1/hubspot/companies/101");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var company = await response.Content.ReadFromJsonAsync<Company>();
        Assert.NotNull(company);
        Assert.Equal(101, company.CompanyId);
        Assert.Equal("HubSpot Co", company.Name);
    }
}
