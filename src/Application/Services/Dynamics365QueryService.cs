using Application.Interfaces;
using Domain.Common;
using Domain.Dynamics365;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class Dynamics365QueryService : IDynamics365QueryService
{
    private readonly IDynamics365Repository _repository;
    private readonly ILogger<Dynamics365QueryService> _logger;

    public Dynamics365QueryService(
        IDynamics365Repository repository,
        ILogger<Dynamics365QueryService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<PagedResult<Account>> GetAccountsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetAccountsAsync(provider, options, ct);

    public Task<Account?> GetAccountByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetAccountByIdAsync(provider, id, ct);

    public Task<IReadOnlyList<Contact>> GetContactsByAccountIdAsync(DatabaseProvider provider, Guid accountId, CancellationToken ct = default) =>
        _repository.GetContactsByAccountIdAsync(provider, accountId, ct);

    public Task<IReadOnlyList<Opportunity>> GetOpportunitiesByAccountIdAsync(DatabaseProvider provider, Guid accountId, CancellationToken ct = default) =>
        _repository.GetOpportunitiesByAccountIdAsync(provider, accountId, ct);

    public Task<PagedResult<Contact>> GetContactsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetContactsAsync(provider, options, ct);

    public Task<Contact?> GetContactByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetContactByIdAsync(provider, id, ct);

    public Task<PagedResult<Opportunity>> GetOpportunitiesAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetOpportunitiesAsync(provider, options, ct);

    public Task<Opportunity?> GetOpportunityByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetOpportunityByIdAsync(provider, id, ct);

    public Task<IReadOnlyList<OpportunityProduct>> GetProductsByOpportunityIdAsync(DatabaseProvider provider, Guid opportunityId, CancellationToken ct = default) =>
        _repository.GetProductsByOpportunityIdAsync(provider, opportunityId, ct);

    public Task<PagedResult<Lead>> GetLeadsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetLeadsAsync(provider, options, ct);

    public Task<Lead?> GetLeadByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetLeadByIdAsync(provider, id, ct);

    public Task<PagedResult<Incident>> GetIncidentsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetIncidentsAsync(provider, options, ct);

    public Task<Incident?> GetIncidentByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetIncidentByIdAsync(provider, id, ct);

    public Task<PagedResult<Product>> GetProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetProductsAsync(provider, options, ct);

    public Task<Product?> GetProductByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetProductByIdAsync(provider, id, ct);

    public Task<PagedResult<PriceLevel>> GetPriceLevelsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetPriceLevelsAsync(provider, options, ct);

    public Task<PriceLevel?> GetPriceLevelByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetPriceLevelByIdAsync(provider, id, ct);

    public Task<PagedResult<OpportunityProduct>> GetOpportunityProductsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetOpportunityProductsAsync(provider, options, ct);

    public Task<OpportunityProduct?> GetOpportunityProductByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetOpportunityProductByIdAsync(provider, id, ct);

    public Task<PagedResult<BusinessUnit>> GetBusinessUnitsAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetBusinessUnitsAsync(provider, options, ct);

    public Task<BusinessUnit?> GetBusinessUnitByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetBusinessUnitByIdAsync(provider, id, ct);

    public Task<PagedResult<SystemUser>> GetSystemUsersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetSystemUsersAsync(provider, options, ct);

    public Task<SystemUser?> GetSystemUserByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetSystemUserByIdAsync(provider, id, ct);

    public Task<PagedResult<ActivityPointer>> GetActivityPointersAsync(DatabaseProvider provider, QueryOptions options, CancellationToken ct = default) =>
        _repository.GetActivityPointersAsync(provider, options, ct);

    public Task<ActivityPointer?> GetActivityPointerByIdAsync(DatabaseProvider provider, Guid id, CancellationToken ct = default) =>
        _repository.GetActivityPointerByIdAsync(provider, id, ct);
}
