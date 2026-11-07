using System.Collections.Immutable;
using Domain.Common;
using Domain.Dynamics365;

namespace Infrastructure.Repositories;

public sealed class Dynamics365Repository : IDynamics365Repository
{
    private readonly IBaseRepository _baseRepository;
    private readonly ICrmIdGenerator _idGenerator;
    private readonly TimeProvider _timeProvider;

    public Dynamics365Repository(IBaseRepository baseRepository, ICrmIdGenerator idGenerator, TimeProvider timeProvider)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public Task<PagedResult<Account>> GetAccountsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Account>(provider, "Dynamics365.Account", options, "accountid", null, ct);

    public Task<Account?> GetAccountByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Account>(provider, "Dynamics365.Account", "accountid", id, ct);

    public Task<IReadOnlyList<Contact>> GetContactsByAccountIdAsync(DatabaseProvider provider, Guid accountId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<Contact>(provider, "Dynamics365.Contact", q => q.Where("parentcustomerid", accountId), ct);

    public Task<IReadOnlyList<Opportunity>> GetOpportunitiesByAccountIdAsync(DatabaseProvider provider, Guid accountId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<Opportunity>(provider, "Dynamics365.Opportunity", q => q.Where("parentaccountid", accountId), ct);

    public Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Contact>(provider, "Dynamics365.Contact", options, "contactid", null, ct);

    public Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Contact>(provider, "Dynamics365.Contact", "contactid", id, ct);

    public Task<PagedResult<Opportunity>> GetOpportunitiesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Opportunity>(provider, "Dynamics365.Opportunity", options, "opportunityid", null, ct);

    public Task<Opportunity?> GetOpportunityByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Opportunity>(provider, "Dynamics365.Opportunity", "opportunityid", id, ct);

    public Task<IReadOnlyList<OpportunityProduct>> GetProductsByOpportunityIdAsync(DatabaseProvider provider, Guid opportunityId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<OpportunityProduct>(provider, "Dynamics365.OpportunityProduct", q => q.Where("opportunityid", opportunityId), ct);

    public Task<PagedResult<Lead>> GetLeadsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Lead>(provider, "Dynamics365.Lead", options, "leadid", null, ct);

    public Task<Lead?> GetLeadByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Lead>(provider, "Dynamics365.Lead", "leadid", id, ct);

    public Task<PagedResult<Incident>> GetIncidentsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Incident>(provider, "Dynamics365.Incident", options, "incidentid", null, ct);

    public Task<Incident?> GetIncidentByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Incident>(provider, "Dynamics365.Incident", "incidentid", id, ct);

    public Task<PagedResult<Product>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Product>(provider, "Dynamics365.Product", options, "productid", null, ct);

    public Task<Product?> GetProductByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Product>(provider, "Dynamics365.Product", "productid", id, ct);

    public Task<PagedResult<PriceLevel>> GetPriceLevelsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<PriceLevel>(provider, "Dynamics365.PriceLevel", options, "pricelevelid", null, ct);

    public Task<PriceLevel?> GetPriceLevelByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<PriceLevel>(provider, "Dynamics365.PriceLevel", "pricelevelid", id, ct);

    public Task<PagedResult<OpportunityProduct>> GetOpportunityProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<OpportunityProduct>(provider, "Dynamics365.OpportunityProduct", options, "opportunityproductid", null, ct);

    public Task<OpportunityProduct?> GetOpportunityProductByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<OpportunityProduct>(provider, "Dynamics365.OpportunityProduct", "opportunityproductid", id, ct);

    public Task<PagedResult<BusinessUnit>> GetBusinessUnitsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<BusinessUnit>(provider, "Dynamics365.BusinessUnit", options, "businessunitid", null, ct);

    public Task<BusinessUnit?> GetBusinessUnitByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<BusinessUnit>(provider, "Dynamics365.BusinessUnit", "businessunitid", id, ct);

    public Task<PagedResult<SystemUser>> GetSystemUsersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<SystemUser>(provider, "Dynamics365.SystemUser", options, "systemuserid", null, ct);

    public Task<SystemUser?> GetSystemUserByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<SystemUser>(provider, "Dynamics365.SystemUser", "systemuserid", id, ct);

    public Task<PagedResult<ActivityPointer>> GetActivityPointersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<ActivityPointer>(provider, "Dynamics365.ActivityPointer", options, "activityid", null, ct);

    public Task<ActivityPointer?> GetActivityPointerByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<ActivityPointer>(provider, "Dynamics365.ActivityPointer", "activityid", id, ct);

    public Task<Guid> ExecuteCreateAccountAsync(DatabaseProvider provider, AccountWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, Dynamics365WriteColumns.AccountTable, Dynamics365WriteColumns.AccountIdColumn, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteUpdateAccountAsync(DatabaseProvider provider, Guid id, AccountWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, Dynamics365WriteColumns.AccountTable, Dynamics365WriteColumns.AccountIdColumn, id, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteDeleteAccountAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, Dynamics365WriteColumns.AccountTable, Dynamics365WriteColumns.AccountIdColumn, id, ct);

    public Task<Guid> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, Dynamics365WriteColumns.ContactTable, Dynamics365WriteColumns.ContactIdColumn, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteUpdateContactAsync(DatabaseProvider provider, Guid id, ContactWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, Dynamics365WriteColumns.ContactTable, Dynamics365WriteColumns.ContactIdColumn, id, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, Dynamics365WriteColumns.ContactTable, Dynamics365WriteColumns.ContactIdColumn, id, ct);

    public Task<Guid> ExecuteCreateOpportunityAsync(DatabaseProvider provider, OpportunityWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, Dynamics365WriteColumns.OpportunityTable, Dynamics365WriteColumns.OpportunityIdColumn, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteUpdateOpportunityAsync(DatabaseProvider provider, Guid id, OpportunityWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, Dynamics365WriteColumns.OpportunityTable, Dynamics365WriteColumns.OpportunityIdColumn, id, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteDeleteOpportunityAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, Dynamics365WriteColumns.OpportunityTable, Dynamics365WriteColumns.OpportunityIdColumn, id, ct);

    private async Task<Guid> CreateAsync(
        DatabaseProvider provider,
        string table,
        string idColumn,
        ImmutableDictionary<string, object?> columns,
        Guid ownerId,
        CancellationToken ct)
    {
        var id = _idGenerator.NewDynamicsId();
        var withAudit = columns.WithCreateAudit(idColumn, id, ownerId, _timeProvider.GetUtcNow().UtcDateTime);
        await _baseRepository.ExecuteInsertAsync(provider, table, withAudit, ct);
        return id;
    }

    private Task<bool> UpdateAsync(
        DatabaseProvider provider,
        string table,
        string idColumn,
        Guid id,
        ImmutableDictionary<string, object?> columns,
        Guid ownerId,
        CancellationToken ct) =>
        _baseRepository.ExecuteUpdateAsync(
            provider,
            table,
            idColumn,
            id,
            columns.WithUpdateAudit(ownerId, _timeProvider.GetUtcNow().UtcDateTime),
            ct);
}
