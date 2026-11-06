using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using D365Account = Domain.Dynamics365.Account;
using D365Contact = Domain.Dynamics365.Contact;
using HsContact = Domain.HubSpot.Contact;
using SfAccount = Domain.Salesforce.Account;
using SfContact = Domain.Salesforce.Contact;

namespace Api.GraphQL;

public sealed class Query
{
    // Salesforce
    public async Task<IReadOnlyList<SfAccount>> GetSalesforceAccounts(
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await queryService.GetAccountsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<SfAccount?> GetSalesforceAccountById(
        string id,
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default) =>
        queryService.GetAccountByIdAsync(provider, id, ct);

    public async Task<IReadOnlyList<SfContact>> GetSalesforceContacts(
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await queryService.GetContactsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<SfContact?> GetSalesforceContactById(
        string id,
        [Service] ISalesforceQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default) =>
        queryService.GetContactByIdAsync(provider, id, ct);

    // HubSpot
    public async Task<IReadOnlyList<Company>> GetHubSpotCompanies(
        [Service] IHubSpotQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await queryService.GetCompaniesAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<Company?> GetHubSpotCompanyById(
        long id,
        [Service] IHubSpotQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default) =>
        queryService.GetCompanyByIdAsync(provider, id, ct);

    public async Task<IReadOnlyList<HsContact>> GetHubSpotContacts(
        [Service] IHubSpotQueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await queryService.GetContactsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    // Dynamics 365
    public async Task<IReadOnlyList<D365Account>> GetDynamics365Accounts(
        [Service] IDynamics365QueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await queryService.GetAccountsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }

    public Task<D365Account?> GetDynamics365AccountById(
        Guid id,
        [Service] IDynamics365QueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        CancellationToken ct = default) =>
        queryService.GetAccountByIdAsync(provider, id, ct);

    public async Task<IReadOnlyList<D365Contact>> GetDynamics365Contacts(
        [Service] IDynamics365QueryService queryService,
        DatabaseProvider provider = DatabaseProvider.Ms,
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default)
    {
        var result = await queryService.GetContactsAsync(provider, new QueryOptions(page, pageSize), ct);
        return result.Items;
    }
}
