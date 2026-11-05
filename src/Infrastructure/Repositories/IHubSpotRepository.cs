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
    Task<Pipeline?> GetPipelineByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);
    Task<IReadOnlyList<DealStage>> GetStagesByPipelineIdAsync(DatabaseProvider provider, long pipelineId, CancellationToken ct = default);

    Task<PagedResult<DealStage>> GetDealStagesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<DealStage?> GetDealStageByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<Engagement>> GetEngagementsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Engagement?> GetEngagementByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);

    Task<PagedResult<ObjectAssociation>> GetObjectAssociationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<ObjectAssociation?> GetObjectAssociationByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default);
}
