using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using Infrastructure.Repositories;

namespace Application.Services;

public sealed class HubSpotQueryService : IHubSpotQueryService
{
    private readonly IHubSpotRepository _repository;

    public HubSpotQueryService(IHubSpotRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<PagedResult<Company>> GetCompaniesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetCompaniesAsync(provider, options, ct);

    public Task<Company?> GetCompanyByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetCompanyByIdAsync(provider, id, ct);

    public Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetContactsAsync(provider, options, ct);

    public Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetContactByIdAsync(provider, id, ct);

    public Task<PagedResult<Deal>> GetDealsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetDealsAsync(provider, options, ct);

    public Task<PagedResult<Deal>> GetDealsByCompanyIdAsync(DatabaseProvider provider, long companyId, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetDealsByCompanyIdAsync(provider, companyId, options, ct);

    public Task<Deal?> GetDealByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetDealByIdAsync(provider, id, ct);

    public Task<IReadOnlyList<LineItem>> GetLineItemsByDealIdAsync(DatabaseProvider provider, long dealId, CancellationToken ct = default) =>
        _repository.GetLineItemsByDealIdAsync(provider, dealId, ct);

    public Task<PagedResult<Ticket>> GetTicketsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetTicketsAsync(provider, options, ct);

    public Task<Ticket?> GetTicketByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetTicketByIdAsync(provider, id, ct);

    public Task<PagedResult<Owner>> GetOwnersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetOwnersAsync(provider, options, ct);

    public Task<Owner?> GetOwnerByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetOwnerByIdAsync(provider, id, ct);

    public Task<PagedResult<Product>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetProductsAsync(provider, options, ct);

    public Task<Product?> GetProductByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetProductByIdAsync(provider, id, ct);

    public Task<PagedResult<LineItem>> GetLineItemsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetLineItemsAsync(provider, options, ct);

    public Task<LineItem?> GetLineItemByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetLineItemByIdAsync(provider, id, ct);

    public Task<PagedResult<Pipeline>> GetPipelinesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetPipelinesAsync(provider, options, ct);

    public Task<Pipeline?> GetPipelineByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetPipelineByIdAsync(provider, id, ct);

    public Task<IReadOnlyList<DealStage>> GetStagesByPipelineIdAsync(DatabaseProvider provider, string pipelineId, CancellationToken ct = default) =>
        _repository.GetStagesByPipelineIdAsync(provider, pipelineId, ct);

    public Task<PagedResult<DealStage>> GetDealStagesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetDealStagesAsync(provider, options, ct);

    public Task<DealStage?> GetDealStageByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetDealStageByIdAsync(provider, id, ct);

    public Task<PagedResult<Engagement>> GetEngagementsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetEngagementsAsync(provider, options, ct);

    public Task<Engagement?> GetEngagementByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetEngagementByIdAsync(provider, id, ct);

    public Task<PagedResult<ObjectAssociation>> GetObjectAssociationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetObjectAssociationsAsync(provider, options, ct);

    public Task<ObjectAssociation?> GetObjectAssociationByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.GetObjectAssociationByIdAsync(provider, id, ct);
}
