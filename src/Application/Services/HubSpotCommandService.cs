using Application.Interfaces;
using Domain.Common;
using Domain.HubSpot;
using Infrastructure.Repositories;

namespace Application.Services;

public sealed class HubSpotCommandService : IHubSpotCommandService
{
    private readonly IHubSpotRepository _repository;

    public HubSpotCommandService(IHubSpotRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Company> ExecuteCreateCompanyAsync(DatabaseProvider provider, CompanyWriteRequest request, CancellationToken ct = default)
    {
        var id = await _repository.ExecuteCreateCompanyAsync(provider, request, ct);
        return await _repository.GetCompanyByIdAsync(provider, id, ct)
            ?? throw new InvalidOperationException("The created Company could not be read back.");
    }

    public async Task<Company?> ExecuteUpdateCompanyAsync(DatabaseProvider provider, long id, CompanyWriteRequest request, CancellationToken ct = default) =>
        await _repository.ExecuteUpdateCompanyAsync(provider, id, request, ct)
            ? await _repository.GetCompanyByIdAsync(provider, id, ct)
            : null;

    public Task<bool> ExecuteDeleteCompanyAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.ExecuteDeleteCompanyAsync(provider, id, ct);

    public async Task<Contact> ExecuteCreateContactAsync(DatabaseProvider provider, ContactWriteRequest request, CancellationToken ct = default)
    {
        var id = await _repository.ExecuteCreateContactAsync(provider, request, ct);
        return await _repository.GetContactByIdAsync(provider, id, ct)
            ?? throw new InvalidOperationException("The created Contact could not be read back.");
    }

    public async Task<Contact?> ExecuteUpdateContactAsync(DatabaseProvider provider, long id, ContactWriteRequest request, CancellationToken ct = default) =>
        await _repository.ExecuteUpdateContactAsync(provider, id, request, ct)
            ? await _repository.GetContactByIdAsync(provider, id, ct)
            : null;

    public Task<bool> ExecuteDeleteContactAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.ExecuteDeleteContactAsync(provider, id, ct);

    public async Task<Deal> ExecuteCreateDealAsync(DatabaseProvider provider, DealWriteRequest request, CancellationToken ct = default)
    {
        var id = await _repository.ExecuteCreateDealAsync(provider, request, ct);
        return await _repository.GetDealByIdAsync(provider, id, ct)
            ?? throw new InvalidOperationException("The created Deal could not be read back.");
    }

    public async Task<Deal?> ExecuteUpdateDealAsync(DatabaseProvider provider, long id, DealWriteRequest request, CancellationToken ct = default) =>
        await _repository.ExecuteUpdateDealAsync(provider, id, request, ct)
            ? await _repository.GetDealByIdAsync(provider, id, ct)
            : null;

    public Task<bool> ExecuteDeleteDealAsync(DatabaseProvider provider, long id, CancellationToken ct = default) =>
        _repository.ExecuteDeleteDealAsync(provider, id, ct);

}
