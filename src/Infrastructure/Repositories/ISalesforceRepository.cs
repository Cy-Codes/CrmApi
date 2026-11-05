using Domain.Common;
using Domain.Salesforce;
using SfTask = Domain.Salesforce.Task;

namespace Infrastructure.Repositories;

public interface ISalesforceRepository
{
    Task<PagedResult<Account>> GetAccountsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Account?> GetAccountByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);
    Task<IReadOnlyList<Contact>> GetContactsByAccountIdAsync(DatabaseProvider provider, string accountId, CancellationToken ct = default);
    Task<IReadOnlyList<Opportunity>> GetOpportunitiesByAccountIdAsync(DatabaseProvider provider, string accountId, CancellationToken ct = default);

    Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<Opportunity>> GetOpportunitiesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Opportunity?> GetOpportunityByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);
    Task<IReadOnlyList<OpportunityLineItem>> GetLineItemsByOpportunityIdAsync(DatabaseProvider provider, string opportunityId, CancellationToken ct = default);

    Task<PagedResult<Lead>> GetLeadsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Lead?> GetLeadByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<Case>> GetCasesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Case?> GetCaseByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<User>> GetUsersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<User?> GetUserByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<Campaign>> GetCampaignsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Campaign?> GetCampaignByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);
    Task<IReadOnlyList<CampaignMember>> GetMembersByCampaignIdAsync(DatabaseProvider provider, string campaignId, CancellationToken ct = default);

    Task<PagedResult<SfTask>> GetTasksAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<SfTask?> GetTaskByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<Event>> GetEventsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Event?> GetEventByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<Product2>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Product2?> GetProductByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<Pricebook2>> GetPricebooksAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<Pricebook2?> GetPricebookByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<PricebookEntry>> GetPricebookEntriesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<PricebookEntry?> GetPricebookEntryByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<OpportunityLineItem>> GetOpportunityLineItemsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<OpportunityLineItem?> GetOpportunityLineItemByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<AccountContactRelation>> GetAccountContactRelationsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<AccountContactRelation?> GetAccountContactRelationByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);

    Task<PagedResult<CampaignMember>> GetCampaignMembersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default);
    Task<CampaignMember?> GetCampaignMemberByIdAsync(DatabaseProvider provider, string id, CancellationToken ct = default);
}
