using Application.Interfaces;
using Domain.Common;
using Domain.Dynamics365;
using Domain.HubSpot;
using Domain.Salesforce;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SfAccount = Domain.Salesforce.Account;
using SfContact = Domain.Salesforce.Contact;
using D365Account = Domain.Dynamics365.Account;

namespace Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ISalesforceQueryService> SalesforceQueryServiceMock { get; } = new();
    public Mock<IHubSpotQueryService> HubSpotQueryServiceMock { get; } = new();
    public Mock<IDynamics365QueryService> Dynamics365QueryServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing service registrations
            var sfDesc = services.SingleOrDefault(d => d.ServiceType == typeof(ISalesforceQueryService));
            if (sfDesc != null) services.Remove(sfDesc);

            var hsDesc = services.SingleOrDefault(d => d.ServiceType == typeof(IHubSpotQueryService));
            if (hsDesc != null) services.Remove(hsDesc);

            var d365Desc = services.SingleOrDefault(d => d.ServiceType == typeof(IDynamics365QueryService));
            if (d365Desc != null) services.Remove(d365Desc);

            // Setup default mock behaviors
            SalesforceQueryServiceMock
                .Setup(s => s.GetAccountsAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<SfAccount>(
                    new List<SfAccount> { new() { Id = "001000000000001AAA", Name = "Acme Corp", Industry = "Technology" } },
                    1, 1, 20));

            SalesforceQueryServiceMock
                .Setup(s => s.GetAccountByIdAsync(It.IsAny<DatabaseProvider>(), "001000000000001AAA", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SfAccount { Id = "001000000000001AAA", Name = "Acme Corp", Industry = "Technology" });

            SalesforceQueryServiceMock
                .Setup(s => s.GetContactsByAccountIdAsync(It.IsAny<DatabaseProvider>(), "001000000000001AAA", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SfContact> { new() { Id = "003000000000001AAA", AccountId = "001000000000001AAA", LastName = "Doe", FirstName = "John" } });

            HubSpotQueryServiceMock
                .Setup(s => s.GetCompaniesAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<Company>(
                    new List<Company> { new() { CompanyId = 101, Name = "HubSpot Co", Domain = "hubspot.com" } },
                    1, 1, 20));

            HubSpotQueryServiceMock
                .Setup(s => s.GetCompanyByIdAsync(It.IsAny<DatabaseProvider>(), 101, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Company { CompanyId = 101, Name = "HubSpot Co", Domain = "hubspot.com" });

            Dynamics365QueryServiceMock
                .Setup(s => s.GetAccountsAsync(It.IsAny<DatabaseProvider>(), It.IsAny<QueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<D365Account>(
                    new List<D365Account> { new() { AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "D365 Account" } },
                    1, 1, 20));

            Dynamics365QueryServiceMock
                .Setup(s => s.GetAccountByIdAsync(It.IsAny<DatabaseProvider>(), Guid.Parse("11111111-1111-1111-1111-111111111111"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new D365Account { AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "D365 Account" });

            // Add mocks to DI
            services.AddSingleton(SalesforceQueryServiceMock.Object);
            services.AddSingleton(HubSpotQueryServiceMock.Object);
            services.AddSingleton(Dynamics365QueryServiceMock.Object);
        });
    }
}
