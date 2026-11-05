using Application.Interfaces;
using Domain.Common;
using Domain.Dynamics365;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/dynamics365")]
[Produces("application/json")]
public class Dynamics365Controller : BaseCrmController
{
    private readonly IDynamics365QueryService _queryService;

    public Dynamics365Controller(IDynamics365QueryService queryService)
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

    [HttpGet("accounts/{id:guid}")]
    public async Task<ActionResult<Account>> GetAccountById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var account = await _queryService.GetAccountByIdAsync(db, id, ct);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpGet("accounts/{id:guid}/contacts")]
    public async Task<ActionResult<IReadOnlyList<Contact>>> GetContactsByAccountId(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetContactsByAccountIdAsync(db, id, ct));
    }

    [HttpGet("accounts/{id:guid}/opportunities")]
    public async Task<ActionResult<IReadOnlyList<Opportunity>>> GetOpportunitiesByAccountId(string provider, Guid id, CancellationToken ct = default)
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

    [HttpGet("contacts/{id:guid}")]
    public async Task<ActionResult<Contact>> GetContactById(string provider, Guid id, CancellationToken ct = default)
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

    [HttpGet("opportunities/{id:guid}")]
    public async Task<ActionResult<Opportunity>> GetOpportunityById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var opp = await _queryService.GetOpportunityByIdAsync(db, id, ct);
        return opp is null ? NotFound() : Ok(opp);
    }

    [HttpGet("opportunities/{id:guid}/products")]
    public async Task<ActionResult<IReadOnlyList<OpportunityProduct>>> GetProductsByOpportunityId(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetProductsByOpportunityIdAsync(db, id, ct));
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

    [HttpGet("leads/{id:guid}")]
    public async Task<ActionResult<Lead>> GetLeadById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var lead = await _queryService.GetLeadByIdAsync(db, id, ct);
        return lead is null ? NotFound() : Ok(lead);
    }

    [HttpGet("incidents")]
    public async Task<ActionResult<PagedResult<Incident>>> GetIncidents(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetIncidentsAsync(db, options, ct));
    }

    [HttpGet("incidents/{id:guid}")]
    public async Task<ActionResult<Incident>> GetIncidentById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var inc = await _queryService.GetIncidentByIdAsync(db, id, ct);
        return inc is null ? NotFound() : Ok(inc);
    }

    [HttpGet("products")]
    public async Task<ActionResult<PagedResult<Product>>> GetProducts(
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

    [HttpGet("products/{id:guid}")]
    public async Task<ActionResult<Product>> GetProductById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var prod = await _queryService.GetProductByIdAsync(db, id, ct);
        return prod is null ? NotFound() : Ok(prod);
    }

    [HttpGet("price-levels")]
    public async Task<ActionResult<PagedResult<PriceLevel>>> GetPriceLevels(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetPriceLevelsAsync(db, options, ct));
    }

    [HttpGet("price-levels/{id:guid}")]
    public async Task<ActionResult<PriceLevel>> GetPriceLevelById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var pl = await _queryService.GetPriceLevelByIdAsync(db, id, ct);
        return pl is null ? NotFound() : Ok(pl);
    }

    [HttpGet("opportunity-products")]
    public async Task<ActionResult<PagedResult<OpportunityProduct>>> GetOpportunityProducts(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetOpportunityProductsAsync(db, options, ct));
    }

    [HttpGet("opportunity-products/{id:guid}")]
    public async Task<ActionResult<OpportunityProduct>> GetOpportunityProductById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var op = await _queryService.GetOpportunityProductByIdAsync(db, id, ct);
        return op is null ? NotFound() : Ok(op);
    }

    [HttpGet("business-units")]
    public async Task<ActionResult<PagedResult<BusinessUnit>>> GetBusinessUnits(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetBusinessUnitsAsync(db, options, ct));
    }

    [HttpGet("business-units/{id:guid}")]
    public async Task<ActionResult<BusinessUnit>> GetBusinessUnitById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var bu = await _queryService.GetBusinessUnitByIdAsync(db, id, ct);
        return bu is null ? NotFound() : Ok(bu);
    }

    [HttpGet("system-users")]
    public async Task<ActionResult<PagedResult<SystemUser>>> GetSystemUsers(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetSystemUsersAsync(db, options, ct));
    }

    [HttpGet("system-users/{id:guid}")]
    public async Task<ActionResult<SystemUser>> GetSystemUserById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var su = await _queryService.GetSystemUserByIdAsync(db, id, ct);
        return su is null ? NotFound() : Ok(su);
    }

    [HttpGet("activity-pointers")]
    public async Task<ActionResult<PagedResult<ActivityPointer>>> GetActivityPointers(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetActivityPointersAsync(db, options, ct));
    }

    [HttpGet("activity-pointers/{id:guid}")]
    public async Task<ActionResult<ActivityPointer>> GetActivityPointerById(string provider, Guid id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var act = await _queryService.GetActivityPointerByIdAsync(db, id, ct);
        return act is null ? NotFound() : Ok(act);
    }
}
