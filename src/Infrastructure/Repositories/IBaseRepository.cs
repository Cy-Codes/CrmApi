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

    /// <summary>
    /// Side effect: inserts one row using only parameterized values; column names must be safe identifiers.
    /// Throws <see cref="ConflictException"/> on foreign-key or unique-key violations.
    /// </summary>
    Task ExecuteInsertAsync(
        DatabaseProvider provider,
        string table,
        IReadOnlyDictionary<string, object?> values,
        CancellationToken ct = default);

    /// <summary>
    /// Side effect: updates the row identified by its identifier column; returns <c>false</c> when no row matched.
    /// Throws <see cref="ConflictException"/> on foreign-key or unique-key violations.
    /// </summary>
    Task<bool> ExecuteUpdateAsync(
        DatabaseProvider provider,
        string table,
        string idColumnName,
        object idValue,
        IReadOnlyDictionary<string, object?> values,
        CancellationToken ct = default);

    /// <summary>
    /// Side effect: hard-deletes the row identified by its identifier column; returns <c>false</c> when no row matched.
    /// Throws <see cref="ConflictException"/> when the row is still referenced.
    /// </summary>
    Task<bool> ExecuteDeleteAsync(
        DatabaseProvider provider,
        string table,
        string idColumnName,
        object idValue,
        CancellationToken ct = default);
}
