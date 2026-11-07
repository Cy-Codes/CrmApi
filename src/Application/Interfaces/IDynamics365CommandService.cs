using Domain.Common;
using Domain.Dynamics365;

namespace Application.Interfaces;

/// <summary>
/// Side-effecting command boundary for Dynamics365 create, update, and delete operations.
/// Queries live in <see cref="IDynamics365QueryService"/>.
/// </summary>
public interface IDynamics365CommandService
{
    /// <summary>Creates a Account and returns the persisted entity.</summary>
    Task<Account> ExecuteCreateAccountAsync(DatabaseProvider provider, AccountWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Account's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Account?> ExecuteUpdateAccountAsync(DatabaseProvider provider, Guid id, AccountWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Account; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteAccountAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    /// <summary>Creates a Contact and returns the persisted entity.</summary>
    Task<Contact> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Contact's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Contact?> ExecuteUpdateContactAsync(DatabaseProvider provider, Guid id, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Contact; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);

    /// <summary>Creates a Opportunity and returns the persisted entity.</summary>
    Task<Opportunity> ExecuteCreateOpportunityAsync(DatabaseProvider provider, OpportunityWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Opportunity's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Opportunity?> ExecuteUpdateOpportunityAsync(DatabaseProvider provider, Guid id, OpportunityWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Opportunity; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteOpportunityAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default);
}
