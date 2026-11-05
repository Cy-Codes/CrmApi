using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using Domain.Common;
using Infrastructure.Compilers;
using Infrastructure.Connections;
using SqlKata;

namespace Infrastructure.Repositories;

/// <summary>
/// Generic repository utilizing SqlKata query compilation and Dapper execution across database engines.
/// </summary>
public partial class BaseRepository : IBaseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ISqlKataCompilerFactory _compilerFactory;

    [GeneratedRegex("^[a-zA-Z0-9_.]+$")]
    private static partial Regex SafeIdentifierRegex();

    public BaseRepository(
        IDbConnectionFactory connectionFactory,
        ISqlKataCompilerFactory compilerFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _compilerFactory = compilerFactory ?? throw new ArgumentNullException(nameof(compilerFactory));
    }

    public async Task<PagedResult<T>> GetPagedAsync<T>(
        DatabaseProvider provider,
        string table,
        QueryOptions options,
        string defaultSortColumn = "Id",
        Func<Query, Query>? configureQuery = null,
        CancellationToken ct = default)
    {
        var page = options.Page < 1 ? 1 : options.Page;
        var pageSize = options.PageSize is < 1 or > 100 ? 20 : options.PageSize;
        var compiler = _compilerFactory.GetCompiler(provider);

        // 1. Build Base Query for Filtering
        var baseQuery = new Query(table);
        ApplyFilters(baseQuery, options.Filters);
        if (configureQuery != null)
        {
            baseQuery = configureQuery(baseQuery);
        }

        // 2. Count Query
        var countQuery = baseQuery.Clone().AsCount();
        var compiledCount = compiler.Compile(countQuery);

        // 3. Data Query with Sorting and Pagination
        var dataQuery = baseQuery.Clone();
        ApplySorting(dataQuery, options.SortBy, options.SortDirection, defaultSortColumn);
        dataQuery.Skip((page - 1) * pageSize).Take(pageSize);
        var compiledData = compiler.Compile(dataQuery);

        // 4. Execute via Dapper
        await using var connection = _connectionFactory.CreateConnection(provider);
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(ct);
        }

        var totalCount = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                compiledCount.Sql,
                new DynamicParameters(compiledCount.NamedBindings),
                cancellationToken: ct));

        var items = await connection.QueryAsync<T>(
            new CommandDefinition(
                compiledData.Sql,
                new DynamicParameters(compiledData.NamedBindings),
                cancellationToken: ct));

        return new PagedResult<T>(items.ToList(), totalCount, page, pageSize);
    }

    public async Task<T?> GetByIdAsync<T>(
        DatabaseProvider provider,
        string table,
        string idColumnName,
        object idValue,
        CancellationToken ct = default)
    {
        if (!SafeIdentifierRegex().IsMatch(idColumnName))
        {
            throw new ArgumentException("Invalid identifier column name", nameof(idColumnName));
        }

        var query = new Query(table).Where(idColumnName, idValue);
        var compiler = _compilerFactory.GetCompiler(provider);
        var compiled = compiler.Compile(query);

        await using var connection = _connectionFactory.CreateConnection(provider);
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(ct);
        }

        return await connection.QueryFirstOrDefaultAsync<T>(
            new CommandDefinition(
                compiled.Sql,
                new DynamicParameters(compiled.NamedBindings),
                cancellationToken: ct));
    }

    public async Task<IReadOnlyList<T>> GetListAsync<T>(
        DatabaseProvider provider,
        string table,
        Func<Query, Query>? configureQuery = null,
        CancellationToken ct = default)
    {
        var query = new Query(table);
        if (configureQuery != null)
        {
            query = configureQuery(query);
        }

        var compiler = _compilerFactory.GetCompiler(provider);
        var compiled = compiler.Compile(query);

        await using var connection = _connectionFactory.CreateConnection(provider);
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(ct);
        }

        var results = await connection.QueryAsync<T>(
            new CommandDefinition(
                compiled.Sql,
                new DynamicParameters(compiled.NamedBindings),
                cancellationToken: ct));

        return results.ToList();
    }

    private static void ApplyFilters(Query query, Dictionary<string, string?>? filters)
    {
        if (filters == null || filters.Count == 0) return;

        foreach (var (key, value) in filters)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            // Guard against SQL injection in column name
            if (!SafeIdentifierRegex().IsMatch(key))
            {
                continue;
            }

            if (value.Contains('%'))
            {
                query.WhereLike(key, value);
            }
            else if (bool.TryParse(value, out var boolVal))
            {
                query.Where(key, boolVal);
            }
            else if (int.TryParse(value, out var intVal))
            {
                query.Where(key, intVal);
            }
            else if (long.TryParse(value, out var longVal))
            {
                query.Where(key, longVal);
            }
            else if (decimal.TryParse(value, out var decVal))
            {
                query.Where(key, decVal);
            }
            else if (Guid.TryParse(value, out var guidVal))
            {
                query.Where(key, guidVal);
            }
            else
            {
                query.Where(key, value);
            }
        }
    }

    private static void ApplySorting(
        Query query,
        string? sortBy,
        string sortDirection,
        string defaultSortColumn)
    {
        var sortCol = !string.IsNullOrWhiteSpace(sortBy) && SafeIdentifierRegex().IsMatch(sortBy)
            ? sortBy
            : defaultSortColumn;

        var isDesc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        if (isDesc)
        {
            query.OrderByDesc(sortCol);
        }
        else
        {
            query.OrderBy(sortCol);
        }
    }
}
