using Api.Extensions;
using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Side-effecting HubSpot endpoints. Create and update require the <c>admin</c> or <c>user</c> role; delete requires <c>admin</c>.
/// Read endpoints stay anonymous in <see cref="HubSpotController"/>.
/// </summary>
[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/hubspot")]
[Produces("application/json")]
public sealed class HubSpotCommandsController : BaseCrmController
{
    private readonly IHubSpotCommandService _commandService;

    public HubSpotCommandsController(IHubSpotCommandService commandService)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    [HttpPost("companies")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Company>> CreateCompany(
        string provider,
        [FromBody] CompanyWriteRequest request,
        CancellationToken ct = default)
    {
        var created = await _commandService.ExecuteCreateCompanyAsync(provider.ToDatabaseProvider(), request, ct);
        return Created($"{Request.Path.Value!.TrimEnd('/')}/{created.CompanyId}", created);
    }

    [HttpPut("companies/{id:long}")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    public Task<ActionResult<Company>> UpdateCompany(
        string provider,
        long id,
        [FromBody] CompanyWriteRequest request,
        CancellationToken ct = default) =>
        _commandService.ExecuteUpdateCompanyAsync(provider.ToDatabaseProvider(), id, request, ct).ToActionResultAsync();

    [HttpDelete("companies/{id:long}")]
    [Authorize(Policy = CrmRoles.DeletePolicy)]
    public Task<ActionResult> DeleteCompany(string provider, long id, CancellationToken ct = default) =>
        _commandService.ExecuteDeleteCompanyAsync(provider.ToDatabaseProvider(), id, ct).ToNoContentOrNotFoundAsync();

    [HttpPost("contacts")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Contact>> CreateContact(
        string provider,
        [FromBody] ContactWriteRequest request,
        CancellationToken ct = default)
    {
        var created = await _commandService.ExecuteCreateContactAsync(provider.ToDatabaseProvider(), request, ct);
        return Created($"{Request.Path.Value!.TrimEnd('/')}/{created.ContactId}", created);
    }

    [HttpPut("contacts/{id:long}")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    public Task<ActionResult<Contact>> UpdateContact(
        string provider,
        long id,
        [FromBody] ContactWriteRequest request,
        CancellationToken ct = default) =>
        _commandService.ExecuteUpdateContactAsync(provider.ToDatabaseProvider(), id, request, ct).ToActionResultAsync();

    [HttpDelete("contacts/{id:long}")]
    [Authorize(Policy = CrmRoles.DeletePolicy)]
    public Task<ActionResult> DeleteContact(string provider, long id, CancellationToken ct = default) =>
        _commandService.ExecuteDeleteContactAsync(provider.ToDatabaseProvider(), id, ct).ToNoContentOrNotFoundAsync();

    [HttpPost("deals")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Deal>> CreateDeal(
        string provider,
        [FromBody] DealWriteRequest request,
        CancellationToken ct = default)
    {
        var created = await _commandService.ExecuteCreateDealAsync(provider.ToDatabaseProvider(), request, ct);
        return Created($"{Request.Path.Value!.TrimEnd('/')}/{created.DealId}", created);
    }

    [HttpPut("deals/{id:long}")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    public Task<ActionResult<Deal>> UpdateDeal(
        string provider,
        long id,
        [FromBody] DealWriteRequest request,
        CancellationToken ct = default) =>
        _commandService.ExecuteUpdateDealAsync(provider.ToDatabaseProvider(), id, request, ct).ToActionResultAsync();

    [HttpDelete("deals/{id:long}")]
    [Authorize(Policy = CrmRoles.DeletePolicy)]
    public Task<ActionResult> DeleteDeal(string provider, long id, CancellationToken ct = default) =>
        _commandService.ExecuteDeleteDealAsync(provider.ToDatabaseProvider(), id, ct).ToNoContentOrNotFoundAsync();
}
