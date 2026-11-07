using System.Collections.Immutable;
using Domain.Common;
using Domain.Salesforce;
using SfTask = Domain.Salesforce.Task;

namespace Infrastructure.Repositories;

public sealed class SalesforceRepository : ISalesforceRepository
{
    private readonly IBaseRepository _baseRepository;
    private readonly ICrmIdGenerator _idGenerator;
    private readonly TimeProvider _timeProvider;

    public SalesforceRepository(IBaseRepository baseRepository, ICrmIdGenerator idGenerator, TimeProvider timeProvider)
    {
        _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public Task<PagedResult<Account>> GetAccountsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Account>(provider, "Salesforce.Account", options, "Id", null, ct);

    public Task<Account?> GetAccountByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Account>(provider, "Salesforce.Account", "Id", id, ct);

    public Task<IReadOnlyList<Contact>> GetContactsByAccountIdAsync(DatabaseProvider provider, string accountId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<Contact>(provider, "Salesforce.Contact", q => q.Where("AccountId", accountId), ct);

    public Task<IReadOnlyList<Opportunity>> GetOpportunitiesByAccountIdAsync(DatabaseProvider provider, string accountId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<Opportunity>(provider, "Salesforce.Opportunity", q => q.Where("AccountId", accountId), ct);

    public Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Contact>(provider, "Salesforce.Contact", options, "Id", null, ct);

    public Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Contact>(provider, "Salesforce.Contact", "Id", id, ct);

    public Task<PagedResult<Opportunity>> GetOpportunitiesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Opportunity>(provider, "Salesforce.Opportunity", options, "Id", null, ct);

    public Task<Opportunity?> GetOpportunityByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Opportunity>(provider, "Salesforce.Opportunity", "Id", id, ct);

    public Task<IReadOnlyList<OpportunityLineItem>> GetLineItemsByOpportunityIdAsync(DatabaseProvider provider, string opportunityId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<OpportunityLineItem>(provider, "Salesforce.OpportunityLineItem", q => q.Where("OpportunityId", opportunityId), ct);

    public Task<PagedResult<Lead>> GetLeadsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Lead>(provider, "Salesforce.Lead", options, "Id", null, ct);

    public Task<Lead?> GetLeadByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Lead>(provider, "Salesforce.Lead", "Id", id, ct);

    public Task<PagedResult<Case>> GetCasesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Case>(provider, "Salesforce.Case", options, "Id", null, ct);

    public Task<Case?> GetCaseByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Case>(provider, "Salesforce.Case", "Id", id, ct);

    public Task<PagedResult<User>> GetUsersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<User>(provider, "Salesforce.User", options, "Id", null, ct);

    public Task<User?> GetUserByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<User>(provider, "Salesforce.User", "Id", id, ct);

    public Task<PagedResult<Campaign>> GetCampaignsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Campaign>(provider, "Salesforce.Campaign", options, "Id", null, ct);

    public Task<Campaign?> GetCampaignByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Campaign>(provider, "Salesforce.Campaign", "Id", id, ct);

    public Task<IReadOnlyList<CampaignMember>> GetMembersByCampaignIdAsync(DatabaseProvider provider, string campaignId, CancellationToken ct = default) =>
        _baseRepository.GetListAsync<CampaignMember>(provider, "Salesforce.CampaignMember", q => q.Where("CampaignId", campaignId), ct);

    public Task<PagedResult<SfTask>> GetTasksAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<SfTask>(provider, "Salesforce.Task", options, "Id", null, ct);

    public Task<SfTask?> GetTaskByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<SfTask>(provider, "Salesforce.Task", "Id", id, ct);

    public Task<PagedResult<Event>> GetEventsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Event>(provider, "Salesforce.Event", options, "Id", null, ct);

    public Task<Event?> GetEventByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Event>(provider, "Salesforce.Event", "Id", id, ct);

    public Task<PagedResult<Product2>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Product2>(provider, "Salesforce.Product2", options, "Id", null, ct);

    public Task<Product2?> GetProductByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Product2>(provider, "Salesforce.Product2", "Id", id, ct);

    public Task<PagedResult<Pricebook2>> GetPricebooksAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<Pricebook2>(provider, "Salesforce.Pricebook2", options, "Id", null, ct);

    public Task<Pricebook2?> GetPricebookByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<Pricebook2>(provider, "Salesforce.Pricebook2", "Id", id, ct);

    public Task<PagedResult<PricebookEntry>> GetPricebookEntriesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<PricebookEntry>(provider, "Salesforce.PricebookEntry", options, "Id", null, ct);

    public Task<PricebookEntry?> GetPricebookEntryByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<PricebookEntry>(provider, "Salesforce.PricebookEntry", "Id", id, ct);

    public Task<PagedResult<OpportunityLineItem>> GetOpportunityLineItemsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<OpportunityLineItem>(provider, "Salesforce.OpportunityLineItem", options, "Id", null, ct);

    public Task<OpportunityLineItem?> GetOpportunityLineItemByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<OpportunityLineItem>(provider, "Salesforce.OpportunityLineItem", "Id", id, ct);

    public Task<PagedResult<AccountContactRelation>> GetAccountContactRelationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<AccountContactRelation>(provider, "Salesforce.AccountContactRelation", options, "Id", null, ct);

    public Task<AccountContactRelation?> GetAccountContactRelationByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<AccountContactRelation>(provider, "Salesforce.AccountContactRelation", "Id", id, ct);

    public Task<PagedResult<CampaignMember>> GetCampaignMembersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _baseRepository.GetPagedAsync<CampaignMember>(provider, "Salesforce.CampaignMember", options, "Id", null, ct);

    public Task<CampaignMember?> GetCampaignMemberByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.GetByIdAsync<CampaignMember>(provider, "Salesforce.CampaignMember", "Id", id, ct);

    public Task<string> ExecuteCreateAccountAsync(DatabaseProvider provider, AccountWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, SalesforceWriteColumns.AccountTable, SalesforceWriteColumns.AccountKeyPrefix, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteUpdateAccountAsync(DatabaseProvider provider, string id, AccountWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, SalesforceWriteColumns.AccountTable, id, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteDeleteAccountAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, SalesforceWriteColumns.AccountTable, SalesforceWriteColumns.IdColumn, id, ct);

    public Task<string> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, SalesforceWriteColumns.ContactTable, SalesforceWriteColumns.ContactKeyPrefix, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteUpdateContactAsync(DatabaseProvider provider, string id, ContactWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, SalesforceWriteColumns.ContactTable, id, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, SalesforceWriteColumns.ContactTable, SalesforceWriteColumns.IdColumn, id, ct);

    public Task<string> ExecuteCreateOpportunityAsync(DatabaseProvider provider, OpportunityWriteRequest request, CancellationToken ct = default) =>
        CreateAsync(provider, SalesforceWriteColumns.OpportunityTable, SalesforceWriteColumns.OpportunityKeyPrefix, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteUpdateOpportunityAsync(DatabaseProvider provider, string id, OpportunityWriteRequest request, CancellationToken ct = default) =>
        UpdateAsync(provider, SalesforceWriteColumns.OpportunityTable, id, request.ToColumns(), request.OwnerId, ct);

    public Task<bool> ExecuteDeleteOpportunityAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _baseRepository.ExecuteDeleteAsync(provider, SalesforceWriteColumns.OpportunityTable, SalesforceWriteColumns.IdColumn, id, ct);

    private async Task<string> CreateAsync(
        DatabaseProvider provider,
        string table,
        string keyPrefix,
        ImmutableDictionary<string, object?> columns,
        string ownerId,
        CancellationToken ct)
    {
        var id = _idGenerator.NewSalesforceId(keyPrefix);
        var withAudit = columns.WithCreateAudit(id, ownerId, _timeProvider.GetUtcNow().UtcDateTime);
        await _baseRepository.ExecuteInsertAsync(provider, table, withAudit, ct);
        return id;
    }

    private Task<bool> UpdateAsync(
        DatabaseProvider provider,
        string table,
        string id,
        ImmutableDictionary<string, object?> columns,
        string ownerId,
        CancellationToken ct) =>
        _baseRepository.ExecuteUpdateAsync(
            provider,
            table,
            SalesforceWriteColumns.IdColumn,
            id,
            columns.WithUpdateAudit(ownerId, _timeProvider.GetUtcNow().UtcDateTime),
            ct);
}
