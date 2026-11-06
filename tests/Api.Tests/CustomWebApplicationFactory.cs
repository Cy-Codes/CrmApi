using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using D365Account = Domain.Dynamics365.Account;
using SfAccount = Domain.Salesforce.Account;
using SfContact = Domain.Salesforce.Contact;

namespace Api.Tests;

public static class ServiceCollectionTestExtensions
{
    /// <summary>
    /// Extension method replacing an existing service registration with a singleton <see cref="Mock{T}"/> instance.
    /// </summary>
    public static IServiceCollection ReplaceWithSingletonMock<TService>(
        this IServiceCollection services,
        Mock<TService> mock)
        where TService : class
    {
        services.RemoveAll<TService>();
        services.AddSingleton(mock.Object);
        return services;
    }
}

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string SampleSalesforceAccountId = "001000000000001AAA";
    public const string SampleSalesforceContactId = "003000000000001AAA";
    public const long SampleHubSpotCompanyId = 101;
    public static readonly Guid SampleDynamicsAccountId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public Mock<ISalesforceQueryService> SalesforceQueryServiceMock { get; } = new();
    public Mock<IHubSpotQueryService> HubSpotQueryServiceMock { get; } = new();
    public Mock<IDynamics365QueryService> Dynamics365QueryServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var sampleSfAccount = new SfAccount
            {
                Id = SampleSalesforceAccountId,
                Name = "Acme Corp",
                Industry = "Technology"
            };

            SalesforceQueryServiceMock
                .Setup(s => s.GetAccountsAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<SfAccount>(
                    [sampleSfAccount],
                    1,
                    QueryOptions.DefaultPage,
                    QueryOptions.DefaultPageSize));

            SalesforceQueryServiceMock
                .Setup(s => s.GetAccountByIdAsync(It.IsAny<DatabaseProvider>(), SampleSalesforceAccountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sampleSfAccount);

            SalesforceQueryServiceMock
                .Setup(s => s.GetContactsByAccountIdAsync(It.IsAny<DatabaseProvider>(), SampleSalesforceAccountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync([
                    new SfContact
                    {
                        Id = SampleSalesforceContactId,
                        AccountId = SampleSalesforceAccountId,
                        LastName = "Doe",
                        FirstName = "John"
                    }
                ]);

            var sampleHsCompany = new Company
            {
                CompanyId = SampleHubSpotCompanyId,
                Name = "HubSpot Co",
                Domain = "hubspot.com"
            };

            HubSpotQueryServiceMock
                .Setup(s => s.GetCompaniesAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<Company>(
                    [sampleHsCompany],
                    1,
                    QueryOptions.DefaultPage,
                    QueryOptions.DefaultPageSize));

            HubSpotQueryServiceMock
                .Setup(s => s.GetCompanyByIdAsync(It.IsAny<DatabaseProvider>(), SampleHubSpotCompanyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sampleHsCompany);

            var sampleD365Account = new D365Account
            {
                AccountId = SampleDynamicsAccountId,
                Name = "D365 Account"
            };

            Dynamics365QueryServiceMock
                .Setup(s => s.GetAccountsAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<D365Account>(
                    [sampleD365Account],
                    1,
                    QueryOptions.DefaultPage,
                    QueryOptions.DefaultPageSize));

            Dynamics365QueryServiceMock
                .Setup(s => s.GetAccountByIdAsync(It.IsAny<DatabaseProvider>(), SampleDynamicsAccountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sampleD365Account);

            services
                .ReplaceWithSingletonMock(SalesforceQueryServiceMock)
                .ReplaceWithSingletonMock(HubSpotQueryServiceMock)
                .ReplaceWithSingletonMock(Dynamics365QueryServiceMock);
        });
    }
}
