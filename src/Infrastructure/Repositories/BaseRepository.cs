using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Dapper;
using Domain.Common;
using Infrastructure.Compilers;
using Infrastructure.Connections;
using SqlKata;

namespace Infrastructure.Repositories;

/// <summary>
/// Generic repository utilizing SqlKata query compilation and Dapper execution across database engines.
/// </summary>
public sealed class BaseRepository : IBaseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ISqlKataCompilerFactory _compilerFactory;

    public BaseRepository(
        IDbConnectionFactory connectionFactory,
        ISqlKataCompilerFactory compilerFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _compilerFactory = compilerFactory ?? throw new ArgumentNullException(nameof(compilerFactory));
    }

    public Task<PagedResult<T>> GetPagedAsync<T>(
        DatabaseProvider provider,
        string table,
        QueryOptions options,
        string defaultSortColumn = QueryOptions.DefaultIdColumn,
        Func<Query, Query>? configureQuery = null,
        CancellationToken ct = default)
    {
        var page = options.NormalizedPage;
        var pageSize = options.NormalizedPageSize;
        var compiler = _compilerFactory.GetCompiler(provider);

        // 1. Pure Query AST Construction: Base Filtering
        var baseQuery = new Query(table).ApplyFilters(options.Filters);
        if (configureQuery is not null)
        {
            baseQuery = configureQuery(baseQuery);
        }

        // 2. Pure Query AST Compilation: Count & Data Queries
        var compiledCount = compiler.Compile(baseQuery.Clone().AsCount());
        var compiledData = compiler.Compile(
            baseQuery.Clone()
                .ApplySorting(options.SortBy, options.SortDirection, defaultSortColumn)
                .ApplyPagination(page, pageSize));

        // 3. Explicit Side-Effecting I/O Execution
        return ExecuteWithOpenConnectionAsync(
            provider,
            async connection =>
            {
                var totalCount = await connection.ExecuteScalarAsync<int>(
                    CreateCommand(compiledCount, ct));

                var items = await connection.QueryAsync<T>(
                    CreateCommand(compiledData, ct));

                return new PagedResult<T>(items.ToImmutableArray(), totalCount, page, pageSize);
            },
            ct);
    }

    public Task<T?> GetByIdAsync<T>(
        DatabaseProvider provider,
        string table,
        string idColumnName,
        object idValue,
        CancellationToken ct = default)
    {
        if (!idColumnName.IsSafeSqlIdentifier())
        {
            throw new ArgumentException("Invalid identifier column name.", nameof(idColumnName));
        }

        var query = new Query(table).Where(idColumnName, idValue);
        var compiled = _compilerFactory.GetCompiler(provider).Compile(query);

        return ExecuteWithOpenConnectionAsync(
            provider,
            connection => connection.QueryFirstOrDefaultAsync<T>(CreateCommand(compiled, ct)),
            ct);
    }

    public Task<IReadOnlyList<T>> GetListAsync<T>(
        DatabaseProvider provider,
        string table,
        Func<Query, Query>? configureQuery = null,
        CancellationToken ct = default)
    {
        var baseQuery = new Query(table);
        var query = configureQuery is not null ? configureQuery(baseQuery) : baseQuery;
        var compiled = _compilerFactory.GetCompiler(provider).Compile(query);

        return ExecuteWithOpenConnectionAsync<IReadOnlyList<T>>(
            provider,
            async connection =>
            {
                var results = await connection.QueryAsync<T>(CreateCommand(compiled, ct));
                return results.ToImmutableArray();
            },
            ct);
    }

    /// <summary>
    /// Pure helper constructing a parameterized Dapper <see cref="CommandDefinition"/> from a compiled SqlKata query.
    /// </summary>
    private static CommandDefinition CreateCommand(SqlResult compiledQuery, CancellationToken ct) =>
        new(
            compiledQuery.Sql,
            new DynamicParameters(compiledQuery.NamedBindings),
            cancellationToken: ct);

    /// <summary>
    /// Side-effecting I/O boundary: opens a provider-specific <see cref="DbConnection"/>, executes the async operation, and disposes the connection.
    /// </summary>
    private async Task<TResult> ExecuteWithOpenConnectionAsync<TResult>(
        DatabaseProvider provider,
        Func<DbConnection, Task<TResult>> operation,
        CancellationToken ct)
    {
        await using var connection = _connectionFactory.CreateConnection(provider);
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(ct);
        }

        return await operation(connection);
    }
}
