using Domain.Common;
using Domain.Dynamics365;

namespace Infrastructure.Repositories;

public interface IDynamics365Repository
{
    Task<PagedResult<Account>> GetAccountsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Account?> GetAccountByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Contact>> GetContactsByAccountIdAsync(DatabaseProvider provider, Guid accountId, CancellationToken ct = default);
    Task<IReadOnlyList<Opportunity>> GetOpportunitiesByAccountIdAsync(DatabaseProvider provider, Guid accountId, CancellationToken ct = default);

    Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<Opportunity>> GetOpportunitiesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Opportunity?> GetOpportunityByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<OpportunityProduct>> GetProductsByOpportunityIdAsync(DatabaseProvider provider, Guid opportunityId, CancellationToken ct = default);

    Task<PagedResult<Lead>> GetLeadsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Lead?> GetLeadByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<Incident>> GetIncidentsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Incident?> GetIncidentByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<Product>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Product?> GetProductByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<PriceLevel>> GetPriceLevelsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<PriceLevel?> GetPriceLevelByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<OpportunityProduct>> GetOpportunityProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<OpportunityProduct?> GetOpportunityProductByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<BusinessUnit>> GetBusinessUnitsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<BusinessUnit?> GetBusinessUnitByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<SystemUser>> GetSystemUsersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<SystemUser?> GetSystemUserByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    Task<PagedResult<ActivityPointer>> GetActivityPointersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<ActivityPointer?> GetActivityPointerByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    /// <summary>Side effect: creates an Account with a server-generated identifier and returns that identifier.</summary>
    Task<Guid> ExecuteCreateAccountAsync(DatabaseProvider provider, AccountWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: fully replaces an Account's writable fields; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteUpdateAccountAsync(DatabaseProvider provider, Guid id, AccountWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: hard-deletes an Account; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteAccountAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    /// <summary>Side effect: creates a Contact with a server-generated identifier and returns that identifier.</summary>
    Task<Guid> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: fully replaces a Contact's writable fields; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteUpdateContactAsync(DatabaseProvider provider, Guid id, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: hard-deletes a Contact; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    /// <summary>Side effect: creates an Opportunity with a server-generated identifier and returns that identifier.</summary>
    Task<Guid> ExecuteCreateOpportunityAsync(DatabaseProvider provider, OpportunityWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: fully replaces an Opportunity's writable fields; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteUpdateOpportunityAsync(DatabaseProvider provider, Guid id, OpportunityWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: hard-deletes an Opportunity; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteOpportunityAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);
}
