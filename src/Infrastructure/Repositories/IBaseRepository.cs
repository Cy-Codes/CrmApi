using Domain.Common;
using SqlKata;

namespace Infrastructure.Repositories;

/// <summary>
/// Generic repository interface for executing compiled SqlKata queries via Dapper.
/// </summary>
public interface IBaseRepository
{
    /// <summary>
    /// Executes a paginated query against the specified database provider.
    /// </summary>
    Task<PagedResult<T>> GetPagedAsync<T>(
        DatabaseProvider provider,
        string table,
        QueryOptions options,
        string defaultSortColumn = "Id",
        Func<Query, Query>? configureQuery = null,
        CancellationToken ct = default);

    /// <summary>
    /// Fetches a single entity by its identifier column.
    /// </summary>
    Task<T?> GetByIdAsync<T>(
        DatabaseProvider provider,
        string table,
        string idColumnName,
        object idValue,
        CancellationToken ct = default);

    /// <summary>
    /// Fetches a list of entities matching query criteria.
    /// </summary>
    Task<IReadOnlyList<T>> GetListAsync<T>(
        DatabaseProvider provider,
        string table,
        Func<Query, Query>? configureQuery = null,
        CancellationToken ct = default);
}
