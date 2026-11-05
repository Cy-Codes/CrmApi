using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/hubspot")]
[Produces("application/json")]
public class HubSpotController : BaseCrmController
{
    private readonly IHubSpotQueryService _queryService;

    public HubSpotController(IHubSpotQueryService queryService)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
    }

    [HttpGet("companies")]
    public async Task<ActionResult<PagedResult<Company>>> GetCompanies(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetCompaniesAsync(db, options, ct));
    }

    [HttpGet("companies/{id:long}")]
    public async Task<ActionResult<Company>> GetCompanyById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var company = await _queryService.GetCompanyByIdAsync(db, id, ct);
        return company is null ? NotFound() : Ok(company);
    }

    [HttpGet("companies/{id:long}/contacts")]
    public async Task<ActionResult<PagedResult<Contact>>> GetContactsByCompanyId(
        string provider,
        long id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = new QueryOptions(page, pageSize, null, "asc", new Dictionary<string, string?> { ["associatedCompanyId"] = id.ToString() });
        return Ok(await _queryService.GetContactsAsync(db, options, ct));
    }

    [HttpGet("companies/{id:long}/deals")]
    public async Task<ActionResult<PagedResult<Deal>>> GetDealsByCompanyId(
        string provider,
        long id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = new QueryOptions(page, pageSize, null, "asc");
        return Ok(await _queryService.GetDealsByCompanyIdAsync(db, id, options, ct));
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

    [HttpGet("contacts/{id:long}")]
    public async Task<ActionResult<Contact>> GetContactById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var contact = await _queryService.GetContactByIdAsync(db, id, ct);
        return contact is null ? NotFound() : Ok(contact);
    }

    [HttpGet("deals")]
    public async Task<ActionResult<PagedResult<Deal>>> GetDeals(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetDealsAsync(db, options, ct));
    }

    [HttpGet("deals/{id:long}")]
    public async Task<ActionResult<Deal>> GetDealById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var deal = await _queryService.GetDealByIdAsync(db, id, ct);
        return deal is null ? NotFound() : Ok(deal);
    }

    [HttpGet("deals/{id:long}/line-items")]
    public async Task<ActionResult<IReadOnlyList<LineItem>>> GetLineItemsByDealId(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetLineItemsByDealIdAsync(db, id, ct));
    }

    [HttpGet("tickets")]
    public async Task<ActionResult<PagedResult<Ticket>>> GetTickets(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetTicketsAsync(db, options, ct));
    }

    [HttpGet("tickets/{id:long}")]
    public async Task<ActionResult<Ticket>> GetTicketById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var ticket = await _queryService.GetTicketByIdAsync(db, id, ct);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpGet("owners")]
    public async Task<ActionResult<PagedResult<Owner>>> GetOwners(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetOwnersAsync(db, options, ct));
    }

    [HttpGet("owners/{id:long}")]
    public async Task<ActionResult<Owner>> GetOwnerById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var owner = await _queryService.GetOwnerByIdAsync(db, id, ct);
        return owner is null ? NotFound() : Ok(owner);
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

    [HttpGet("products/{id:long}")]
    public async Task<ActionResult<Product>> GetProductById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var prod = await _queryService.GetProductByIdAsync(db, id, ct);
        return prod is null ? NotFound() : Ok(prod);
    }

    [HttpGet("line-items")]
    public async Task<ActionResult<PagedResult<LineItem>>> GetLineItems(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetLineItemsAsync(db, options, ct));
    }

    [HttpGet("line-items/{id:long}")]
    public async Task<ActionResult<LineItem>> GetLineItemById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var item = await _queryService.GetLineItemByIdAsync(db, id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("pipelines")]
    public async Task<ActionResult<PagedResult<Pipeline>>> GetPipelines(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetPipelinesAsync(db, options, ct));
    }

    [HttpGet("pipelines/{id}")]
    public async Task<ActionResult<Pipeline>> GetPipelineById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var pipeline = await _queryService.GetPipelineByIdAsync(db, id, ct);
        return pipeline is null ? NotFound() : Ok(pipeline);
    }

    [HttpGet("pipelines/{id}/stages")]
    public async Task<ActionResult<IReadOnlyList<DealStage>>> GetStagesByPipelineId(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        return Ok(await _queryService.GetStagesByPipelineIdAsync(db, id, ct));
    }

    [HttpGet("deal-stages")]
    public async Task<ActionResult<PagedResult<DealStage>>> GetDealStages(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetDealStagesAsync(db, options, ct));
    }

    [HttpGet("deal-stages/{id}")]
    public async Task<ActionResult<DealStage>> GetDealStageById(string provider, string id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var stage = await _queryService.GetDealStageByIdAsync(db, id, ct);
        return stage is null ? NotFound() : Ok(stage);
    }

    [HttpGet("engagements")]
    public async Task<ActionResult<PagedResult<Engagement>>> GetEngagements(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetEngagementsAsync(db, options, ct));
    }

    [HttpGet("engagements/{id:long}")]
    public async Task<ActionResult<Engagement>> GetEngagementById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var eng = await _queryService.GetEngagementByIdAsync(db, id, ct);
        return eng is null ? NotFound() : Ok(eng);
    }

    [HttpGet("object-associations")]
    public async Task<ActionResult<PagedResult<ObjectAssociation>>> GetObjectAssociations(
        string provider,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var options = CreateQueryOptions(page, pageSize, sortBy, sortDirection);
        return Ok(await _queryService.GetObjectAssociationsAsync(db, options, ct));
    }

    [HttpGet("object-associations/{id:long}")]
    public async Task<ActionResult<ObjectAssociation>> GetObjectAssociationById(string provider, long id, CancellationToken ct = default)
    {
        var db = ParseProvider(provider);
        var assoc = await _queryService.GetObjectAssociationByIdAsync(db, id, ct);
        return assoc is null ? NotFound() : Ok(assoc);
    }
}
