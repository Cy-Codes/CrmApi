using Domain.Common;
using Domain.HubSpot;

namespace Application.Interfaces;

/// <summary>
/// Side-effecting command boundary for HubSpot create, update, and delete operations.
/// Queries live in <see cref="IHubSpotQueryService"/>.
/// </summary>
public interface IHubSpotCommandService
{
    /// <summary>Creates a Company and returns the persisted entity.</summary>
    Task<Company> ExecuteCreateCompanyAsync(DatabaseProvider provider, CompanyWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Company's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Company?> ExecuteUpdateCompanyAsync(DatabaseProvider provider, long id, CompanyWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Company; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteCompanyAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    /// <summary>Creates a Contact and returns the persisted entity.</summary>
    Task<Contact> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Contact's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Contact?> ExecuteUpdateContactAsync(DatabaseProvider provider, long id, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Contact; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    /// <summary>Creates a Deal and returns the persisted entity.</summary>
    Task<Deal> ExecuteCreateDealAsync(DatabaseProvider provider, DealWriteRequest request, CancellationToken ct = default);
    /// <summary>Fully replaces a Deal's writable fields; returns <c>null</c> when it does not exist.</summary>
    Task<Deal?> ExecuteUpdateDealAsync(DatabaseProvider provider, long id, DealWriteRequest request, CancellationToken ct = default);
    /// <summary>Deletes a Deal; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteDealAsync(DatabaseProvider provider, long id, CancellationToken ct = default);
}
