using Domain.Common;
using Domain.HubSpot;

namespace Infrastructure.Repositories;

public interface IHubSpotRepository
{
    Task<PagedResult<Company>> GetCompaniesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Company?> GetCompanyByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<Deal>> GetDealsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<PagedResult<Deal>> GetDealsByCompanyIdAsync(DatabaseProvider provider, long companyId, QueryOptions options, CancellationToken ct = default);
    Task<Deal?> GetDealByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);
    Task<IReadOnlyList<LineItem>> GetLineItemsByDealIdAsync(DatabaseProvider provider, long dealId, CancellationToken ct = default);

    Task<PagedResult<Ticket>> GetTicketsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Ticket?> GetTicketByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<Owner>> GetOwnersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Owner?> GetOwnerByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<Product>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Product?> GetProductByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<LineItem>> GetLineItemsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<LineItem?> GetLineItemByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<Pipeline>> GetPipelinesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Pipeline?> GetPipelineByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);
    Task<IReadOnlyList<DealStage>> GetStagesByPipelineIdAsync(DatabaseProvider provider, string pipelineId, CancellationToken ct = default);

    Task<PagedResult<DealStage>> GetDealStagesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<DealStage?> GetDealStageByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<Engagement>> GetEngagementsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Engagement?> GetEngagementByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<ObjectAssociation>> GetObjectAssociationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<ObjectAssociation?> GetObjectAssociationByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    /// <summary>Side effect: creates a Company with a server-generated identifier and returns that identifier.</summary>
    Task<long> ExecuteCreateCompanyAsync(DatabaseProvider provider, CompanyWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: fully replaces a Company's writable fields; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteUpdateCompanyAsync(DatabaseProvider provider, long id, CompanyWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: hard-deletes a Company; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteCompanyAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    /// <summary>Side effect: creates a Contact with a server-generated identifier and returns that identifier.</summary>
    Task<long> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: fully replaces a Contact's writable fields; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteUpdateContactAsync(DatabaseProvider provider, long id, ContactWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: hard-deletes a Contact; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    /// <summary>Side effect: creates a Deal with a server-generated identifier and returns that identifier.</summary>
    Task<long> ExecuteCreateDealAsync(DatabaseProvider provider, DealWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: fully replaces a Deal's writable fields; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteUpdateDealAsync(DatabaseProvider provider, long id, DealWriteRequest request, CancellationToken ct = default);
    /// <summary>Side effect: hard-deletes a Deal; returns <c>false</c> when it does not exist.</summary>
    Task<bool> ExecuteDeleteDealAsync(DatabaseProvider provider, long id, CancellationToken ct = default);
}
