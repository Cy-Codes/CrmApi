using Domain.Common;
using Domain.HubSpot;

namespace Infrastructure.Repositories;

public class HubSpotRepository : IHubSpotRepository
{
    private readonly IBaseRepository _baseRepository;

    public HubSpotRepository(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
    }

    public Task<PagedResult<Company>> GetCompaniesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Company>(provider, "HubSpot.Company", options, "companyId", null, ct);

    public Task<Company?> GetCompanyByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Company>(provider, "HubSpot.Company", "companyId", id, ct);

    public Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Contact>(provider, "HubSpot.Contact", options, "contactId", null, ct);

    public Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Contact>(provider, "HubSpot.Contact", "contactId", id, ct);

    public Task<PagedResult<Deal>> GetDealsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Deal>(provider, "HubSpot.Deal", options, "dealId", null, ct);

    public Task<Deal?> GetDealByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Deal>(provider, "HubSpot.Deal", "dealId", id, ct);

    public Task<IReadOnlyList<LineItem>> GetLineItemsByDealIdAsync(DatabaseProvider provider, long dealId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<LineItem>(provider, "HubSpot.LineItem", q => q.Where("dealId", dealId), ct);

    public Task<PagedResult<Ticket>> GetTicketsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Ticket>(provider, "HubSpot.Ticket", options, "ticketId", null, ct);

    public Task<Ticket?> GetTicketByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Ticket>(provider, "HubSpot.Ticket", "ticketId", id, ct);

    public Task<PagedResult<Owner>> GetOwnersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Owner>(provider, "HubSpot.Owner", options, "ownerId", null, ct);

    public Task<Owner?> GetOwnerByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Owner>(provider, "HubSpot.Owner", "ownerId", id, ct);

    public Task<PagedResult<Product>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Product>(provider, "HubSpot.Product", options, "productId", null, ct);

    public Task<Product?> GetProductByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Product>(provider, "HubSpot.Product", "productId", id, ct);

    public Task<PagedResult<LineItem>> GetLineItemsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<LineItem>(provider, "HubSpot.LineItem", options, "lineItemId", null, ct);

    public Task<LineItem?> GetLineItemByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<LineItem>(provider, "HubSpot.LineItem", "lineItemId", id, ct);

    public Task<PagedResult<Pipeline>> GetPipelinesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Pipeline>(provider, "HubSpot.Pipeline", options, "pipelineId", null, ct);

    public Task<Pipeline?> GetPipelineByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Pipeline>(provider, "HubSpot.Pipeline", "pipelineId", id, ct);

    public Task<IReadOnlyList<DealStage>> GetStagesByPipelineIdAsync(DatabaseProvider provider, long pipelineId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<DealStage>(provider, "HubSpot.DealStage", q => q.Where("pipelineId", pipelineId), ct);

    public Task<PagedResult<DealStage>> GetDealStagesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<DealStage>(provider, "HubSpot.DealStage", options, "stageId", null, ct);

    public Task<DealStage?> GetDealStageByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<DealStage>(provider, "HubSpot.DealStage", "stageId", id, ct);

    public Task<PagedResult<Engagement>> GetEngagementsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Engagement>(provider, "HubSpot.Engagement", options, "engagementId", null, ct);

    public Task<Engagement?> GetEngagementByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Engagement>(provider, "HubSpot.Engagement", "engagementId", id, ct);

    public Task<PagedResult<ObjectAssociation>> GetObjectAssociationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<ObjectAssociation>(provider, "HubSpot.ObjectAssociation", options, "associationId", null, ct);

    public Task<ObjectAssociation?> GetObjectAssociationByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<ObjectAssociation>(provider, "HubSpot.ObjectAssociation", "associationId", id, ct);
}
