using System.Collections.Frozen;
using System.Globalization;
using Api.Extensions;
using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/hubspot")]
[Produces("application/json")]
public sealed class HubSpotController : BaseCrmController
{
    private const string AssociatedCompanyIdFilterKey = "associatedCompanyId";

    private readonly IHubSpotQueryService _queryService;

    public HubSpotController(IHubSpotQueryService queryService)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
    }

    [HttpGet("companies")]
    public Task<ActionResult<PagedResult<Company>>> GetCompanies(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetCompaniesAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("companies/{id:long}")]
    public Task<ActionResult<Company>> GetCompanyById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetCompanyByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("companies/{id:long}/contacts")]
    public Task<ActionResult<PagedResult<Contact>>> GetContactsByCompanyId(
        string provider,
        long id,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default)
    {
        var filters = new Dictionary<string, string?>
        {
            [AssociatedCompanyIdFilterKey] = id.ToString(CultureInfo.InvariantCulture)
        }.ToFrozenDictionary();

        var options = new QueryOptions(page, pageSize, null, QueryOptions.DefaultSortDirection, filters);
        return _queryService.GetContactsAsync(provider.ToDatabaseProvider(), options, ct).ToOkActionResultAsync();
    }

    [HttpGet("companies/{id:long}/deals")]
    public Task<ActionResult<PagedResult<Deal>>> GetDealsByCompanyId(
        string provider,
        long id,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        CancellationToken ct = default) =>
        _queryService
            .GetDealsByCompanyIdAsync(
                provider.ToDatabaseProvider(),
                id,
                new QueryOptions(page, pageSize, null, QueryOptions.DefaultSortDirection),
                ct)
            .ToOkActionResultAsync();

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

    [HttpGet("contacts/{id:long}")]
    public Task<ActionResult<Contact>> GetContactById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetContactByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("deals")]
    public Task<ActionResult<PagedResult<Deal>>> GetDeals(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetDealsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("deals/{id:long}")]
    public Task<ActionResult<Deal>> GetDealById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetDealByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("deals/{id:long}/line-items")]
    public Task<ActionResult<IReadOnlyList<LineItem>>> GetLineItemsByDealId(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetLineItemsByDealIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

    [HttpGet("tickets")]
    public Task<ActionResult<PagedResult<Ticket>>> GetTickets(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetTicketsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("tickets/{id:long}")]
    public Task<ActionResult<Ticket>> GetTicketById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetTicketByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("owners")]
    public Task<ActionResult<PagedResult<Owner>>> GetOwners(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetOwnersAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("owners/{id:long}")]
    public Task<ActionResult<Owner>> GetOwnerById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetOwnerByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

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

    [HttpGet("products/{id:long}")]
    public Task<ActionResult<Product>> GetProductById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetProductByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("line-items")]
    public Task<ActionResult<PagedResult<LineItem>>> GetLineItems(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetLineItemsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("line-items/{id:long}")]
    public Task<ActionResult<LineItem>> GetLineItemById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetLineItemByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("pipelines")]
    public Task<ActionResult<PagedResult<Pipeline>>> GetPipelines(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetPipelinesAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("pipelines/{id}")]
    public Task<ActionResult<Pipeline>> GetPipelineById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetPipelineByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("pipelines/{id}/stages")]
    public Task<ActionResult<IReadOnlyList<DealStage>>> GetStagesByPipelineId(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetStagesByPipelineIdAsync(provider.ToDatabaseProvider(), id, ct).ToOkActionResultAsync();

    [HttpGet("deal-stages")]
    public Task<ActionResult<PagedResult<DealStage>>> GetDealStages(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetDealStagesAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("deal-stages/{id}")]
    public Task<ActionResult<DealStage>> GetDealStageById(string provider, string id, CancellationToken ct = default) =>
        _queryService.GetDealStageByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("engagements")]
    public Task<ActionResult<PagedResult<Engagement>>> GetEngagements(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetEngagementsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("engagements/{id:long}")]
    public Task<ActionResult<Engagement>> GetEngagementById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetEngagementByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();

    [HttpGet("object-associations")]
    public Task<ActionResult<PagedResult<ObjectAssociation>>> GetObjectAssociations(
        string provider,
        [FromQuery] int page = QueryOptions.DefaultPage,
        [FromQuery] int pageSize = QueryOptions.DefaultPageSize,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = QueryOptions.DefaultSortDirection,
        CancellationToken ct = default) =>
        _queryService
            .GetObjectAssociationsAsync(provider.ToDatabaseProvider(), CreateQueryOptions(page, pageSize, sortBy, sortDirection), ct)
            .ToOkActionResultAsync();

    [HttpGet("object-associations/{id:long}")]
    public Task<ActionResult<ObjectAssociation>> GetObjectAssociationById(string provider, long id, CancellationToken ct = default) =>
        _queryService.GetObjectAssociationByIdAsync(provider.ToDatabaseProvider(), id, ct).ToActionResultAsync();
}
