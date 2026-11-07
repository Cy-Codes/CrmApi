using Application.Interfaces;
using Domain.Common;
using Domain.Dynamics365;
using Infrastructure.Repositories;

namespace Application.Services;

public sealed class Dynamics365CommandService : IDynamics365CommandService
{
    private readonly IDynamics365Repository _repository;

    public Dynamics365CommandService(IDynamics365Repository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Account> ExecuteCreateAccountAsync(DatabaseProvider provider, AccountWriteRequest request, CancellationToken ct = default)
    {
        var id = await _repository.ExecuteCreateAccountAsync(provider, request, ct);
        return await _repository.GetAccountByIdAsync(provider, id, ct)
            ?? throw new InvalidOperationException("The created Account could not be read back.");
    }

    public async Task<Account?> ExecuteUpdateAccountAsync(DatabaseProvider provider, Guid id, AccountWriteRequest request, CancellationToken ct = default) =>
        await _repository.ExecuteUpdateAccountAsync(provider, id, request, ct)
            ? await _repository.GetAccountByIdAsync(provider, id, ct)
            : null;

    public Task<bool> ExecuteDeleteAccountAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.ExecuteDeleteAccountAsync(provider, id, ct);

    public async Task<Contact> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default)
    {
        var id = await _repository.ExecuteCreateContactAsync(provider, request, ct);
        return await _repository.GetContactByIdAsync(provider, id, ct)
            ?? throw new InvalidOperationException("The created Contact could not be read back.");
    }

    public async Task<Contact?> ExecuteUpdateContactAsync(DatabaseProvider provider, Guid id, ContactWriteRequest request, CancellationToken ct = default) =>
        await _repository.ExecuteUpdateContactAsync(provider, id, request, ct)
            ? await _repository.GetContactByIdAsync(provider, id, ct)
            : null;

    public Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.ExecuteDeleteContactAsync(provider, id, ct);

    public async Task<Opportunity> ExecuteCreateOpportunityAsync(DatabaseProvider provider, OpportunityWriteRequest request, CancellationToken ct = default)
    {
        var id = await _repository.ExecuteCreateOpportunityAsync(provider, request, ct);
        return await _repository.GetOpportunityByIdAsync(provider, id, ct)
            ?? throw new InvalidOperationException("The created Opportunity could not be read back.");
    }

    public async Task<Opportunity?> ExecuteUpdateOpportunityAsync(DatabaseProvider provider, Guid id, OpportunityWriteRequest request, CancellationToken ct = default) =>
        await _repository.ExecuteUpdateOpportunityAsync(provider, id, request, ct)
            ? await _repository.GetOpportunityByIdAsync(provider, id, ct)
            : null;

    public Task<bool> ExecuteDeleteOpportunityAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.ExecuteDeleteOpportunityAsync(provider, id, ct);

}
