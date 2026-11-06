using Application.Interfaces;
using Domain.Common;
using Domain.Salesforce;
using Infrastructure.Repositories;
using SfTask = Domain.Salesforce.Task;

namespace Application.Services;

public sealed class SalesforceQueryService : ISalesforceQueryService
{
    private readonly ISalesforceRepository _repository;

    public SalesforceQueryService(ISalesforceRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<PagedResult<Account>> GetAccountsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetAccountsAsync(provider, options, ct);

    public Task<Account?> GetAccountByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetAccountByIdAsync(provider, id, ct);

    public Task<IReadOnlyList<Contact>> GetContactsByAccountIdAsync(DatabaseProvider provider, string accountId, CancellationToken ct = default) =>
        _repository.GetContactsByAccountIdAsync(provider, accountId, ct);

    public Task<IReadOnlyList<Opportunity>> GetOpportunitiesByAccountIdAsync(DatabaseProvider provider, string accountId, CancellationToken ct = default) =>
        _repository.GetOpportunitiesByAccountIdAsync(provider, accountId, ct);

    public Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetContactsAsync(provider, options, ct);

    public Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetContactByIdAsync(provider, id, ct);

    public Task<PagedResult<Opportunity>> GetOpportunitiesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetOpportunitiesAsync(provider, options, ct);

    public Task<Opportunity?> GetOpportunityByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetOpportunityByIdAsync(provider, id, ct);

    public Task<IReadOnlyList<OpportunityLineItem>> GetLineItemsByOpportunityIdAsync(DatabaseProvider provider, string opportunityId, CancellationToken ct = default) =>
        _repository.GetLineItemsByOpportunityIdAsync(provider, opportunityId, ct);

    public Task<PagedResult<Lead>> GetLeadsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetLeadsAsync(provider, options, ct);

    public Task<Lead?> GetLeadByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetLeadByIdAsync(provider, id, ct);

    public Task<PagedResult<Case>> GetCasesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetCasesAsync(provider, options, ct);

    public Task<Case?> GetCaseByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetCaseByIdAsync(provider, id, ct);

    public Task<PagedResult<User>> GetUsersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetUsersAsync(provider, options, ct);

    public Task<User?> GetUserByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetUserByIdAsync(provider, id, ct);

    public Task<PagedResult<Campaign>> GetCampaignsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetCampaignsAsync(provider, options, ct);

    public Task<Campaign?> GetCampaignByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetCampaignByIdAsync(provider, id, ct);

    public Task<IReadOnlyList<CampaignMember>> GetMembersByCampaignIdAsync(DatabaseProvider provider, string campaignId, CancellationToken ct = default) =>
        _repository.GetMembersByCampaignIdAsync(provider, campaignId, ct);

    public Task<PagedResult<SfTask>> GetTasksAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetTasksAsync(provider, options, ct);

    public Task<SfTask?> GetTaskByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetTaskByIdAsync(provider, id, ct);

    public Task<PagedResult<Event>> GetEventsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetEventsAsync(provider, options, ct);

    public Task<Event?> GetEventByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetEventByIdAsync(provider, id, ct);

    public Task<PagedResult<Product2>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetProductsAsync(provider, options, ct);

    public Task<Product2?> GetProductByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetProductByIdAsync(provider, id, ct);

    public Task<PagedResult<Pricebook2>> GetPricebooksAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetPricebooksAsync(provider, options, ct);

    public Task<Pricebook2?> GetPricebookByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetPricebookByIdAsync(provider, id, ct);

    public Task<PagedResult<PricebookEntry>> GetPricebookEntriesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetPricebookEntriesAsync(provider, options, ct);

    public Task<PricebookEntry?> GetPricebookEntryByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetPricebookEntryByIdAsync(provider, id, ct);

    public Task<PagedResult<OpportunityLineItem>> GetOpportunityLineItemsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetOpportunityLineItemsAsync(provider, options, ct);

    public Task<OpportunityLineItem?> GetOpportunityLineItemByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetOpportunityLineItemByIdAsync(provider, id, ct);

    public Task<PagedResult<AccountContactRelation>> GetAccountContactRelationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetAccountContactRelationsAsync(provider, options, ct);

    public Task<AccountContactRelation?> GetAccountContactRelationByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetAccountContactRelationByIdAsync(provider, id, ct);

    public Task<PagedResult<CampaignMember>> GetCampaignMembersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetCampaignMembersAsync(provider, options, ct);

    public Task<CampaignMember?> GetCampaignMemberByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default) =>
        _repository.GetCampaignMemberByIdAsync(provider, id, ct);
}
