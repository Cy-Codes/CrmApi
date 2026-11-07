using Domain.Common;
using Domain.Salesforce;

namespace Application.Interfaces;

/// <summary>
/// Side-effecting command boundary for Salesforce create, update, and delete operations.
/// Queries live in <see cref="ISalesforceQueryService"/>.
/// </summary>
public interface ISalesforceCommandService
{
    /// <summary>Creates a Account and returns the persisted entity.</summary>
    Task<Account> ExecuteCreateAccountAsync(DatabaseProvider provider, AccountWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Account's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Account?> ExecuteUpdateAccountAsync(DatabaseProvider provider, string id, AccountWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Account; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteAccountAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    /// <summary>Creates a Contact and returns the persisted entity.</summary>
    Task<Contact> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Contact's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Contact?> ExecuteUpdateContactAsync(DatabaseProvider provider, string id, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Contact; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    /// <summary>Creates a Opportunity and returns the persisted entity.</summary>
    Task<Opportunity> ExecuteCreateOpportunityAsync(DatabaseProvider provider, OpportunityWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Opportunity's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Opportunity?> ExecuteUpdateOpportunityAsync(DatabaseProvider provider, string id, OpportunityWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Opportunity; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteOpportunityAsync(DatabaseProvider provider, string id, CancellationToken ct = default);
}
