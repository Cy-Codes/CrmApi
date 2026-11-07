using Api.Extensions;
using Application.Interfaces;
using Domain.Common;
using Domain.Salesforce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Side-effecting Salesforce endpoints. Create and update require the <c>admin</c> or <c>user</c> role; delete requires <c>admin</c>.
/// Read endpoints stay anonymous in <see cref="SalesforceController"/>.
/// </summary>
[ApiController]
[Route("api/{provider:regex(^(ms|pg)$)}/v1/salesforce")]
[Produces("application/json")]
public sealed class SalesforceCommandsController : BaseCrmController
{
    private readonly ISalesforceCommandService _commandService;

    public SalesforceCommandsController(ISalesforceCommandService commandService)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    [HttpPost("accounts")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Account>> CreateAccount(
        string provider,
        [FromBody] AccountWriteRequest request,
        CancellationToken ct = default)
    {
        var created = await _commandService.ExecuteCreateAccountAsync(provider.ToDatabaseProvider(), request, ct);
        return Created($"{Request.Path.Value!.TrimEnd('/')}/{created.Id}", created);
    }

    [HttpPut("accounts/{id}")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    public Task<ActionResult<Account>> UpdateAccount(
        string provider,
        string id,
        [FromBody] AccountWriteRequest request,
        CancellationToken ct = default) =>
        _commandService.ExecuteUpdateAccountAsync(provider.ToDatabaseProvider(), id, request, ct).ToActionResultAsync();

    [HttpDelete("accounts/{id}")]
    [Authorize(Policy = CrmRoles.DeletePolicy)]
    public Task<ActionResult> DeleteAccount(string provider, string id, CancellationToken ct = default) =>
        _commandService.ExecuteDeleteAccountAsync(provider.ToDatabaseProvider(), id, ct).ToNoContentOrNotFoundAsync();

    [HttpPost("contacts")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Contact>> CreateContact(
        string provider,
        [FromBody] ContactWriteRequest request,
        CancellationToken ct = default)
    {
        var created = await _commandService.ExecuteCreateContactAsync(provider.ToDatabaseProvider(), request, ct);
        return Created($"{Request.Path.Value!.TrimEnd('/')}/{created.Id}", created);
    }

    [HttpPut("contacts/{id}")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    public Task<ActionResult<Contact>> UpdateContact(
        string provider,
        string id,
        [FromBody] ContactWriteRequest request,
        CancellationToken ct = default) =>
        _commandService.ExecuteUpdateContactAsync(provider.ToDatabaseProvider(), id, request, ct).ToActionResultAsync();

    [HttpDelete("contacts/{id}")]
    [Authorize(Policy = CrmRoles.DeletePolicy)]
    public Task<ActionResult> DeleteContact(string provider, string id, CancellationToken ct = default) =>
        _commandService.ExecuteDeleteContactAsync(provider.ToDatabaseProvider(), id, ct).ToNoContentOrNotFoundAsync();

    [HttpPost("opportunities")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Opportunity>> CreateOpportunity(
        string provider,
        [FromBody] OpportunityWriteRequest request,
        CancellationToken ct = default)
    {
        var created = await _commandService.ExecuteCreateOpportunityAsync(provider.ToDatabaseProvider(), request, ct);
        return Created($"{Request.Path.Value!.TrimEnd('/')}/{created.Id}", created);
    }

    [HttpPut("opportunities/{id}")]
    [Authorize(Policy = CrmRoles.WritePolicy)]
    public Task<ActionResult<Opportunity>> UpdateOpportunity(
        string provider,
        string id,
        [FromBody] OpportunityWriteRequest request,
        CancellationToken ct = default) =>
        _commandService.ExecuteUpdateOpportunityAsync(provider.ToDatabaseProvider(), id, request, ct).ToActionResultAsync();

    [HttpDelete("opportunities/{id}")]
    [Authorize(Policy = CrmRoles.DeletePolicy)]
    public Task<ActionResult> DeleteOpportunity(string provider, string id, CancellationToken ct = default) =>
        _commandService.ExecuteDeleteOpportunityAsync(provider.ToDatabaseProvider(), id, ct).ToNoContentOrNotFoundAsync();
}
