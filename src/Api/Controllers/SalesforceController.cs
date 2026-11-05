using Application.Interfaces;
using Domain.Common;
using Domain.Salesforce;
using Microsoft.AspNetCore.Mvc;
using SfTask = Domain.Salesforce.Task;

namespace Api.Controllers;

[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/salesforce")]
[Produces("application/json")]
public class SalesforceController : BaseCrmController
{
    private readonly ISalesforceQueryService _queryService;

    public SalesforceController(ISalesforceQueryService queryService)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
    }

    [HttpGet("accounts")]
    public async Task<ActionResult<PagedResult<Account>>> GetAccounts(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetAccountsAsync(db, options, ct));
    }

    [HttpGet("accounts/{id}")]
    public async Task<ActionResult<Account>> GetAccountById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var account = await _queryService.GetAccountByIdAsync(db, id, ct);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpGet("accounts/{id}/contacts")]
    public async Task<ActionResult<IReadOnlyList<Contact>>> GetContactsByAccountId(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetContactsByAccountIdAsync(db, id, ct));
    }

    [HttpGet("accounts/{id}/opportunities")]
    public async Task<ActionResult<IReadOnlyList<Opportunity>>> GetOpportunitiesByAccountId(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetOpportunitiesByAccountIdAsync(db, id, ct));
    }

    [HttpGet("contacts")]
    public async Task<ActionResult<PagedResult<Contact>>> GetContacts(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetContactsAsync(db, options, ct));
    }

    [HttpGet("contacts/{id}")]
    public async Task<ActionResult<Contact>> GetContactById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var contact = await _queryService.GetContactByIdAsync(db, id, ct);
        return contact is null ? NotFound() : Ok(contact);
    }

    [HttpGet("opportunities")]
    public async Task<ActionResult<PagedResult<Opportunity>>> GetOpportunities(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetOpportunitiesAsync(db, options, ct));
    }

    [HttpGet("opportunities/{id}")]
    public async Task<ActionResult<Opportunity>> GetOpportunityById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var opp = await _queryService.GetOpportunityByIdAsync(db, id, ct);
        return opp is null ? NotFound() : Ok(opp);
    }

    [HttpGet("opportunities/{id}/line-items")]
    public async Task<ActionResult<IReadOnlyList<OpportunityLineItem>>> GetLineItemsByOpportunityId(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetLineItemsByOpportunityIdAsync(db, id, ct));
    }

    [HttpGet("leads")]
    public async Task<ActionResult<PagedResult<Lead>>> GetLeads(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetLeadsAsync(db, options, ct));
    }

    [HttpGet("leads/{id}")]
    public async Task<ActionResult<Lead>> GetLeadById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var lead = await _queryService.GetLeadByIdAsync(db, id, ct);
        return lead is null ? NotFound() : Ok(lead);
    }

    [HttpGet("cases")]
    public async Task<ActionResult<PagedResult<Case>>> GetCases(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetCasesAsync(db, options, ct));
    }

    [HttpGet("cases/{id}")]
    public async Task<ActionResult<Case>> GetCaseById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var c = await _queryService.GetCaseByIdAsync(db, id, ct);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpGet("users")]
    public async Task<ActionResult<PagedResult<User>>> GetUsers(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetUsersAsync(db, options, ct));
    }

    [HttpGet("users/{id}")]
    public async Task<ActionResult<User>> GetUserById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var user = await _queryService.GetUserByIdAsync(db, id, ct);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("campaigns")]
    public async Task<ActionResult<PagedResult<Campaign>>> GetCampaigns(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetCampaignsAsync(db, options, ct));
    }

    [HttpGet("campaigns/{id}")]
    public async Task<ActionResult<Campaign>> GetCampaignById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var campaign = await _queryService.GetCampaignByIdAsync(db, id, ct);
        return campaign is null ? NotFound() : Ok(campaign);
    }

    [HttpGet("campaigns/{id}/members")]
    public async Task<ActionResult<IReadOnlyList<CampaignMember>>> GetMembersByCampaignId(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetMembersByCampaignIdAsync(db, id, ct));
    }

    [HttpGet("tasks")]
    public async Task<ActionResult<PagedResult<SfTask>>> GetTasks(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetTasksAsync(db, options, ct));
    }

    [HttpGet("tasks/{id}")]
    public async Task<ActionResult<SfTask>> GetTaskById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var task = await _queryService.GetTaskByIdAsync(db, id, ct);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpGet("events")]
    public async Task<ActionResult<PagedResult<Event>>> GetEvents(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetEventsAsync(db, options, ct));
    }

    [HttpGet("events/{id}")]
    public async Task<ActionResult<Event>> GetEventById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var evt = await _queryService.GetEventByIdAsync(db, id, ct);
        return evt is null ? NotFound() : Ok(evt);
    }

    [HttpGet("products")]
    public async Task<ActionResult<PagedResult<Product2>>> GetProducts(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetProductsAsync(db, options, ct));
    }

    [HttpGet("products/{id}")]
    public async Task<ActionResult<Product2>> GetProductById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var prod = await _queryService.GetProductByIdAsync(db, id, ct);
        return prod is null ? NotFound() : Ok(prod);
    }

    [HttpGet("pricebooks")]
    public async Task<ActionResult<PagedResult<Pricebook2>>> GetPricebooks(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetPricebooksAsync(db, options, ct));
    }

    [HttpGet("pricebooks/{id}")]
    public async Task<ActionResult<Pricebook2>> GetPricebookById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var pb = await _queryService.GetPricebookByIdAsync(db, id, ct);
        return pb is null ? NotFound() : Ok(pb);
    }

    [HttpGet("pricebook-entries")]
    public async Task<ActionResult<PagedResult<PricebookEntry>>> GetPricebookEntries(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetPricebookEntriesAsync(db, options, ct));
    }

    [HttpGet("pricebook-entries/{id}")]
    public async Task<ActionResult<PricebookEntry>> GetPricebookEntryById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var entry = await _queryService.GetPricebookEntryByIdAsync(db, id, ct);
        return entry is null ? NotFound() : Ok(entry);
    }

    [HttpGet("opportunity-line-items")]
    public async Task<ActionResult<PagedResult<OpportunityLineItem>>> GetOpportunityLineItems(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetOpportunityLineItemsAsync(db, options, ct));
    }

    [HttpGet("opportunity-line-items/{id}")]
    public async Task<ActionResult<OpportunityLineItem>> GetOpportunityLineItemById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var oli = await _queryService.GetOpportunityLineItemByIdAsync(db, id, ct);
        return oli is null ? NotFound() : Ok(oli);
    }

    [HttpGet("account-contact-relations")]
    public async Task<ActionResult<PagedResult<AccountContactRelation>>> GetAccountContactRelations(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetAccountContactRelationsAsync(db, options, ct));
    }

    [HttpGet("account-contact-relations/{id}")]
    public async Task<ActionResult<AccountContactRelation>> GetAccountContactRelationById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var rel = await _queryService.GetAccountContactRelationByIdAsync(db, id, ct);
        return rel is null ? NotFound() : Ok(rel);
    }

    [HttpGet("campaign-members")]
    public async Task<ActionResult<PagedResult<CampaignMember>>> GetCampaignMembers(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetCampaignMembersAsync(db, options, ct));
    }

    [HttpGet("campaign-members/{id}")]
    public async Task<ActionResult<CampaignMember>> GetCampaignMemberById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var cm = await _queryService.GetCampaignMemberByIdAsync(db, id, ct);
        return cm is null ? NotFound() : Ok(cm);
    }
}
