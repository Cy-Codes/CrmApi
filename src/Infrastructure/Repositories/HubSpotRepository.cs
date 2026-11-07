using System.Collections.Immutable;
using Domain.Common;
using Domain.HubSpot;

namespace Infrastructure.Repositories;

public sealed class HubSpotRepository : IHubSpotRepository
{
    private readonly IBaseRepository _baseRepository;
    private readonly ICrmIdGenerator _idGenerator;
    private readonly TimeProvider _timeProvider;

    public HubSpotRepository(IBaseRepository baseRepository, ICrmIdGenerator idGenerator, TimeProvider timeProvider)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
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

    public Task<PagedResult<Deal>> GetDealsByCompanyIdAsync(DatabaseProvider provider, long companyId, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Deal>(
            provider,
            "HubSpot.Deal",
            options,
            "dealId",
            q => q.WhereIn("dealId", new SqlKata.Query("HubSpot.ObjectAssociation")
                .Select("fromObjectId")
                .Where("fromObjectType", "deal")
                .Where("toObjectType", "company")
                .Where("toObjectId", companyId)),
            ct);

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

    public Task<Pipeline?> GetPipelineByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Pipeline>(provider, "HubSpot.Pipeline", "pipelineId", id, ct);

    public Task<IReadOnlyList<DealStage>> GetStagesByPipelineIdAsync(DatabaseProvider provider, string pipelineId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<DealStage>(provider, "HubSpot.DealStage", q => q.Where("pipelineId", pipelineId), ct);

    public Task<PagedResult<DealStage>> GetDealStagesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<DealStage>(provider, "HubSpot.DealStage", options, "stageId", null, ct);

    public Task<DealStage?> GetDealStageByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<DealStage>(provider, "HubSpot.DealStage", "stageId", id, ct);

    public Task<PagedResult<Engagement>> GetEngagementsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Engagement>(provider, "HubSpot.Engagement", options, "engagementId", null, ct);

    public Task<Engagement?> GetEngagementByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Engagement>(provider, "HubSpot.Engagement", "engagementId", id, ct);

    public Task<PagedResult<ObjectAssociation>> GetObjectAssociationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<ObjectAssociation>(provider, "HubSpot.ObjectAssociation", options, "associationId", null, ct);

    public Task<ObjectAssociation?> GetObjectAssociationByIdAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<ObjectAssociation>(provider, "HubSpot.ObjectAssociation", "associationId", id, ct);

    public Task<long> ExecuteCreateCompanyAsync(DatabaseProvider provider, CompanyWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, HubSpotWriteColumns.CompanyTable, HubSpotWriteColumns.CompanyIdColumn, request.ToColumns(), ct);

    public Task<bool> ExecuteUpdateCompanyAsync(DatabaseProvider provider, long id, CompanyWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, HubSpotWriteColumns.CompanyTable, HubSpotWriteColumns.CompanyIdColumn, id, request.ToColumns(), ct);

    public Task<bool> ExecuteDeleteCompanyAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, HubSpotWriteColumns.CompanyTable, HubSpotWriteColumns.CompanyIdColumn, id, ct);

    public Task<long> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, HubSpotWriteColumns.ContactTable, HubSpotWriteColumns.ContactIdColumn, request.ToColumns(), ct);

    public Task<bool> ExecuteUpdateContactAsync(DatabaseProvider provider, long id, ContactWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, HubSpotWriteColumns.ContactTable, HubSpotWriteColumns.ContactIdColumn, id, request.ToColumns(), ct);

    public Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, HubSpotWriteColumns.ContactTable, HubSpotWriteColumns.ContactIdColumn, id, ct);

    public Task<long> ExecuteCreateDealAsync(DatabaseProvider provider, DealWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, HubSpotWriteColumns.DealTable, HubSpotWriteColumns.DealIdColumn, request.ToColumns(), ct);

    public Task<bool> ExecuteUpdateDealAsync(DatabaseProvider provider, long id, DealWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, HubSpotWriteColumns.DealTable, HubSpotWriteColumns.DealIdColumn, id, request.ToColumns(), ct);

    public Task<bool> ExecuteDeleteDealAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, HubSpotWriteColumns.DealTable, HubSpotWriteColumns.DealIdColumn, id, ct);

    private async Task<long> CreateAsync(
        DatabaseProvider provider,
        string table,
        string idColumn,
        ImmutableDictionary<string, object?> columns,
        CancellationToken ct)
    {
        var id = _idGenerator.NewHubSpotId();
        var withAudit = columns.WithCreateAudit(idColumn, id, _timeProvider.GetUtcNow().UtcDateTime);
        await _baseRepository.ExecuteInsertAsync(provider, table, withAudit, ct);
        return id;
    }

    private Task<bool> UpdateAsync(
        DatabaseProvider provider,
        string table,
        string idColumn,
        long id,
        ImmutableDictionary<string, object?> columns,
        CancellationToken ct) =>
        _baseRepository.ExecuteUpdateAsync(
            provider,
            table,
            idColumn,
            id,
            columns.WithUpdateAudit(_timeProvider.GetUtcNow().UtcDateTime),
            ct);
}
