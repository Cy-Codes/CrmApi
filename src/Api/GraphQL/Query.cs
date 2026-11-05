using Application.Interfaces;
using Domain.Common;
using Domain.Dynamics365;
using Domain.HubSpot;
using Domain.Salesforce;
using SfAccount = Domain.Salesforce.Account;
using D365Account = Domain.Dynamics365.Account;

namespace Api.GraphQL;

public class Query
{
    // Salesforce
    public async Task<IReadOnlyList<SfAccount>> GetSalesforceAccounts(
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await queryService.GetAccountsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<SfAccount?> GetSalesforceAccountById(
        string id,
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default)
    {
        return queryService.GetAccountByIdAsync(provider, id, ct);
    }

    public async Task<IReadOnlyList<Domain.Salesforce.Contact>> GetSalesforceContacts(
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await queryService.GetContactsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<Domain.Salesforce.Contact?> GetSalesforceContactById(
        string id,
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default)
    {
        return queryService.GetContactByIdAsync(provider, id, ct);
    }

    // HubSpot
    public async Task<IReadOnlyList<Company>> GetHubSpotCompanies(
        [Service] IHubSpotQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await queryService.GetCompaniesAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<Company?> GetHubSpotCompanyById(
        long id,
        [Service] IHubSpotQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default)
    {
        return queryService.GetCompanyByIdAsync(provider, id, ct);
    }

    public async Task<IReadOnlyList<Domain.HubSpot.Contact>> GetHubSpotContacts(
        [Service] IHubSpotQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await queryService.GetContactsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    // Dynamics 365
    public async Task<IReadOnlyList<D365Account>> GetDynamics365Accounts(
        [Service] IDynamics365QueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await queryService.GetAccountsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<D365Account?> GetDynamics365AccountById(
        Guid id,
        [Service] IDynamics365QueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default)
    {
        return queryService.GetAccountByIdAsync(provider, id, ct);
    }

    public async Task<IReadOnlyList<Domain.Dynamics365.Contact>> GetDynamics365Contacts(
        [Service] IDynamics365QueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await queryService.GetContactsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }
}
