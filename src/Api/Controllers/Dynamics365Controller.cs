using Api.Extensions;
using Application.Interfaces;
using Domain.Common;
using Domain.Dynamics365;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/dynamics365")]
[Produces("application/json")]
public sealed class Dynamics365Controller : BaseCrmController
{
    private readonly IDynamics365QueryService _queryService;

    public Dynamics365Controller(IDynamics365QueryService queryService)
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

    [HttpGet("accounts/{id:guid}")]
    public Task<ActionResult<Account>> GetAccountById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetAccountByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("accounts/{id:guid}/contacts")]
    public Task<ActionResult<IReadOnlyList<Contact>>> GetContactsByAccountId(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetContactsByAccountIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

    [HttpGet("accounts/{id:guid}/opportunities")]
    public Task<ActionResult<IReadOnlyList<Opportunity>>> GetOpportunitiesByAccountId(string provider, Guid id, CancellationToken ct = default) =>
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

    [HttpGet("contacts/{id:guid}")]
    public Task<ActionResult<Contact>> GetContactById(string provider, Guid id, CancellationToken ct = default) =>
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

    [HttpGet("opportunities/{id:guid}")]
    public Task<ActionResult<Opportunity>> GetOpportunityById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetOpportunityByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("opportunities/{id:guid}/products")]
    public Task<ActionResult<IReadOnlyList<OpportunityProduct>>> GetProductsByOpportunityId(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetProductsByOpportunityIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

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

    [HttpGet("leads/{id:guid}")]
    public Task<ActionResult<Lead>> GetLeadById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetLeadByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("incidents")]
    public Task<ActionResult<PagedResult<Incident>>> GetIncidents(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetIncidentsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("incidents/{id:guid}")]
    public Task<ActionResult<Incident>> GetIncidentById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetIncidentByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("products")]
    public Task<ActionResult<PagedResult<Product>>> GetProducts(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetProductsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("products/{id:guid}")]
    public Task<ActionResult<Product>> GetProductById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetProductByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("price-levels")]
    public Task<ActionResult<PagedResult<PriceLevel>>> GetPriceLevels(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetPriceLevelsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("price-levels/{id:guid}")]
    public Task<ActionResult<PriceLevel>> GetPriceLevelById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetPriceLevelByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("opportunity-products")]
    public Task<ActionResult<PagedResult<OpportunityProduct>>> GetOpportunityProducts(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetOpportunityProductsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("opportunity-products/{id:guid}")]
    public Task<ActionResult<OpportunityProduct>> GetOpportunityProductById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetOpportunityProductByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("business-units")]
    public Task<ActionResult<PagedResult<BusinessUnit>>> GetBusinessUnits(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetBusinessUnitsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("business-units/{id:guid}")]
    public Task<ActionResult<BusinessUnit>> GetBusinessUnitById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetBusinessUnitByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("system-users")]
    public Task<ActionResult<PagedResult<SystemUser>>> GetSystemUsers(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetSystemUsersAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("system-users/{id:guid}")]
    public Task<ActionResult<SystemUser>> GetSystemUserById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetSystemUserByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("activity-pointers")]
    public Task<ActionResult<PagedResult<ActivityPointer>>> GetActivityPointers(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetActivityPointersAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("activity-pointers/{id:guid}")]
    public Task<ActionResult<ActivityPointer>> GetActivityPointerById(string provider, Guid id, CancellationToken ct = default) =>
        _queryService.GetActivityPointerByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();
}
