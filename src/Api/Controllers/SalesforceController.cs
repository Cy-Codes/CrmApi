using Api.Extensions;
using Application.Interfaces;
using Domain.Common;
using Domain.Salesforce;
using Microsoft.AspNetCore.Mvc;
using SfTask = Domain.Salesforce.Task;

namespace Api.Controllers;

[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/salesforce")]
[Produces("application/json")]
public sealed class SalesforceController : BaseCrmController
{
    private readonly ISalesforceQueryService _queryService;

    public SalesforceController(ISalesforceQueryService queryService)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
    }

    [HttpGet("accounts")]
    public Task<ActionResult<PagedResult<Account>>> GetAccounts(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetAccountsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("accounts/{id}")]
    public Task<ActionResult<Account>> GetAccountById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetAccountByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("accounts/{id}/contacts")]
    public Task<ActionResult<IReadOnlyList<Contact>>> GetContactsByAccountId(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetContactsByAccountIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

    [HttpGet("accounts/{id}/opportunities")]
    public Task<ActionResult<IReadOnlyList<Opportunity>>> GetOpportunitiesByAccountId(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetOpportunitiesByAccountIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

    [HttpGet("contacts")]
    public Task<ActionResult<PagedResult<Contact>>> GetContacts(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetContactsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("contacts/{id}")]
    public Task<ActionResult<Contact>> GetContactById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetContactByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("opportunities")]
    public Task<ActionResult<PagedResult<Opportunity>>> GetOpportunities(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetOpportunitiesAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("opportunities/{id}")]
    public Task<ActionResult<Opportunity>> GetOpportunityById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetOpportunityByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("opportunities/{id}/line-items")]
    public Task<ActionResult<IReadOnlyList<OpportunityLineItem>>> GetLineItemsByOpportunityId(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetLineItemsByOpportunityIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

    [HttpGet("leads")]
    public Task<ActionResult<PagedResult<Lead>>> GetLeads(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetLeadsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("leads/{id}")]
    public Task<ActionResult<Lead>> GetLeadById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetLeadByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("cases")]
    public Task<ActionResult<PagedResult<Case>>> GetCases(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetCasesAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("cases/{id}")]
    public Task<ActionResult<Case>> GetCaseById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetCaseByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("users")]
    public Task<ActionResult<PagedResult<User>>> GetUsers(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetUsersAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("users/{id}")]
    public Task<ActionResult<User>> GetUserById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetUserByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("campaigns")]
    public Task<ActionResult<PagedResult<Campaign>>> GetCampaigns(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetCampaignsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("campaigns/{id}")]
    public Task<ActionResult<Campaign>> GetCampaignById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetCampaignByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("campaigns/{id}/members")]
    public Task<ActionResult<IReadOnlyList<CampaignMember>>> GetMembersByCampaignId(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetMembersByCampaignIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

    [HttpGet("tasks")]
    public Task<ActionResult<PagedResult<SfTask>>> GetTasks(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetTasksAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("tasks/{id}")]
    public Task<ActionResult<SfTask>> GetTaskById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetTaskByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("events")]
    public Task<ActionResult<PagedResult<Event>>> GetEvents(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetEventsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("events/{id}")]
    public Task<ActionResult<Event>> GetEventById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetEventByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("products")]
    public Task<ActionResult<PagedResult<Product2>>> GetProducts(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetProductsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("products/{id}")]
    public Task<ActionResult<Product2>> GetProductById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetProductByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("pricebooks")]
    public Task<ActionResult<PagedResult<Pricebook2>>> GetPricebooks(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetPricebooksAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("pricebooks/{id}")]
    public Task<ActionResult<Pricebook2>> GetPricebookById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetPricebookByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("pricebook-entries")]
    public Task<ActionResult<PagedResult<PricebookEntry>>> GetPricebookEntries(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetPricebookEntriesAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("pricebook-entries/{id}")]
    public Task<ActionResult<PricebookEntry>> GetPricebookEntryById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetPricebookEntryByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("opportunity-line-items")]
    public Task<ActionResult<PagedResult<OpportunityLineItem>>> GetOpportunityLineItems(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetOpportunityLineItemsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("opportunity-line-items/{id}")]
    public Task<ActionResult<OpportunityLineItem>> GetOpportunityLineItemById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetOpportunityLineItemByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("account-contact-relations")]
    public Task<ActionResult<PagedResult<AccountContactRelation>>> GetAccountContactRelations(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetAccountContactRelationsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("account-contact-relations/{id}")]
    public Task<ActionResult<AccountContactRelation>> GetAccountContactRelationById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetAccountContactRelationByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("campaign-members")]
    public Task<ActionResult<PagedResult<CampaignMember>>> GetCampaignMembers(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetCampaignMembersAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("campaign-members/{id}")]
    public Task<ActionResult<CampaignMember>> GetCampaignMemberById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetCampaignMemberByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();
}
