using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Net.Sockets;
using Dapper;
using Domain.Common;
using Infrastructure.Compilers;
using Infrastructure.Connections;
using Microsoft.Data.SqlClient;
using Npgsql;
using SqlKata;

namespace Infrastructure.Repositories;

/// <summary>
/// Generic repository utilizing SqlKata query compilation and Dapper execution across database engines.
/// </summary>
public sealed class BaseRepository : IBaseRepository
{
    public const int MaxChildResults = 1000;

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
                await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);

                var totalCount = await connection.ExecuteScalarAsync<int>(
                    CreateCommand(compiledCount, ct, transaction));

                var items = await connection.QueryAsync<T>(
                    CreateCommand(compiledData, ct, transaction));

                await transaction.CommitAsync(ct);
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
        var configuredQuery = configureQuery is not null ? configureQuery(baseQuery) : baseQuery;
        var boundedQuery = configuredQuery.Limit(MaxChildResults);
        var compiled = _compilerFactory.GetCompiler(provider).Compile(boundedQuery);

        return ExecuteWithOpenConnectionAsync<IReadOnlyList<T>>(
            provider,
            async connection =>
            {
                var results = await connection.QueryAsync<T>(CreateCommand(compiled, ct));
                return results.ToImmutableArray();
            },
            ct);
    }

    public Task ExecuteInsertAsync(
        DatabaseProvider provider,
        string table,
        IReadOnlyDictionary<string, object?> values,
        CancellationToken ct = default)
    {
        EnsureSafeIdentifiers(table, values.Keys);

        var compiled = _compilerFactory.GetCompiler(provider)
            .Compile(new Query(table).AsInsert(values));

        return ExecuteMutationAsync(provider, compiled, ct);
    }

    public async Task<bool> ExecuteUpdateAsync(
        DatabaseProvider provider,
        string table,
        string idColumnName,
        object idValue,
        IReadOnlyDictionary<string, object?> values,
        CancellationToken ct = default)
    {
        EnsureSafeIdentifiers(table, values.Keys.Append(idColumnName));

        var compiled = _compilerFactory.GetCompiler(provider)
            .Compile(new Query(table).Where(idColumnName, idValue).AsUpdate(values));

        return await ExecuteMutationAsync(provider, compiled, ct) > 0;
    }

    public async Task<bool> ExecuteDeleteAsync(
        DatabaseProvider provider,
        string table,
        string idColumnName,
        object idValue,
        CancellationToken ct = default)
    {
        EnsureSafeIdentifiers(table, [idColumnName]);

        var compiled = _compilerFactory.GetCompiler(provider)
            .Compile(new Query(table).Where(idColumnName, idValue).AsDelete());

        return await ExecuteMutationAsync(provider, compiled, ct) > 0;
    }

    /// <summary>
    /// Pure guard rejecting table or column names that are not whitelisted SQL identifiers.
    /// </summary>
    private static void EnsureSafeIdentifiers(string table, IEnumerable<string> columns)
    {
        if (!table.IsSafeSqlIdentifier())
        {
            throw new ArgumentException("Invalid table name.", nameof(table));
        }

        var unsafeColumn = columns.FirstOrDefault(static column => !column.IsSafeSqlIdentifier());
        if (unsafeColumn is not null)
        {
            throw new ArgumentException($"Invalid column name '{unsafeColumn}'.", nameof(columns));
        }
    }

    /// <summary>
    /// Side-effecting I/O boundary: executes one compiled mutation inside a transaction and translates
    /// constraint violations into <see cref="ConflictException"/>.
    /// </summary>
    private Task<int> ExecuteMutationAsync(DatabaseProvider provider, SqlResult compiled, CancellationToken ct) =>
        ExecuteWithOpenConnectionAsync(
            provider,
            async connection =>
            {
                try
                {
                    await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
                    var affected = await connection.ExecuteAsync(CreateCommand(compiled, ct, transaction));
                    await transaction.CommitAsync(ct);
                    return affected;
                }
                catch (Exception ex) when (IsConstraintViolation(ex))
                {
                    throw new ConflictException("The operation violates a relational constraint (foreign key or unique key).", ex);
                }
            },
            ct,
            retryTransientFaults: false);

    private static bool IsConstraintViolation(Exception ex) =>
        ex switch
        {
            SqlException sqlEx => SqlServerConstraintErrors.Contains(sqlEx.Number),
            PostgresException pgEx => PgConstraintSqlStates.Contains(pgEx.SqlState),
            _ => false
        };

    /// <summary>
    /// Pure helper constructing a parameterized Dapper <see cref="CommandDefinition"/> from a compiled SqlKata query.
    /// </summary>
    private static CommandDefinition CreateCommand(
        SqlResult compiledQuery,
        CancellationToken ct,
        DbTransaction? transaction = null) =>
        new(
            compiledQuery.Sql,
            new DynamicParameters(compiledQuery.NamedBindings),
            transaction: transaction,
            cancellationToken: ct);

    private const int MaxRetryAttempts = 3;
    private static readonly TimeSpan InitialRetryDelay = TimeSpan.FromMilliseconds(50);

    // Well-known transient SQL Server error numbers:
    // 1205: Deadlock victim, -2: Execution timeout, 4060: Cannot open database,
    // 40197: Error processing request, 40501: Service busy, 40613: Database unavailable,
    // 10054/10053: Connection forcibly closed/reset, 233: Connection initialization failure
    private const int SqlServerDeadlockVictim = 1205;
    private const int SqlServerExecutionTimeout = -2;
    private const int SqlServerCannotOpenDatabase = 4060;
    private const int SqlServerServiceProcessingError = 40197;
    private const int SqlServerServiceBusy = 40501;
    private const int SqlServerDatabaseUnavailable = 40613;
    private const int SqlServerConnectionReset = 10054;
    private const int SqlServerConnectionAborted = 10053;
    private const int SqlServerConnectionInitFailure = 233;

    private static readonly ImmutableHashSet<int> TransientSqlServerErrors =
        [
            SqlServerDeadlockVictim,
            SqlServerExecutionTimeout,
            SqlServerCannotOpenDatabase,
            SqlServerServiceProcessingError,
            SqlServerServiceBusy,
            SqlServerDatabaseUnavailable,
            SqlServerConnectionReset,
            SqlServerConnectionAborted,
            SqlServerConnectionInitFailure,
        ];

    // Well-known transient PostgreSQL SQLSTATE codes:
    // 40001: serialization_failure, 57P01: admin_shutdown, 08*: connection_exception family
    private const string PgSqlStateSerializationFailure = "40001";
    private const string PgSqlStateAdminShutdown = "57P01";
    private const string PgConnectionExceptionPrefix = "08";

    // Constraint violations surfaced as HTTP 409:
    // SQL Server 547: FK/CHECK conflict, 2627: unique constraint, 2601: unique index
    // PostgreSQL 23503: foreign_key_violation, 23505: unique_violation
    private const int SqlServerConstraintConflict = 547;
    private const int SqlServerUniqueConstraint = 2627;
    private const int SqlServerUniqueIndex = 2601;
    private const string PgForeignKeyViolation = "23503";
    private const string PgUniqueViolation = "23505";

    private static readonly ImmutableHashSet<int> SqlServerConstraintErrors =
        [SqlServerConstraintConflict, SqlServerUniqueConstraint, SqlServerUniqueIndex];

    private static readonly ImmutableHashSet<string> PgConstraintSqlStates =
        [PgForeignKeyViolation, PgUniqueViolation];

    /// <summary>
    /// Side-effecting I/O boundary: opens a provider-specific <see cref="DbConnection"/>, executes the async operation (with transient fault retries unless disabled for non-idempotent mutations), and disposes the connection.
    /// </summary>
    private async Task<TResult> ExecuteWithOpenConnectionAsync<TResult>(
        DatabaseProvider provider,
        Func<DbConnection, Task<TResult>> operation,
        CancellationToken ct,
        bool retryTransientFaults = true)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await using var connection = _connectionFactory.CreateConnection(provider);
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(ct);
                }

                return await operation(connection);
            }
            catch (Exception ex) when (retryTransientFaults && attempt < MaxRetryAttempts && !ct.IsCancellationRequested && IsTransientException(ex))
            {
                var delay = TimeSpan.FromMilliseconds(InitialRetryDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                await Task.Delay(delay, ct);
            }
        }
    }

    private static bool IsTransientException(Exception ex) =>
        ex switch
        {
            SqlException sqlEx => sqlEx.IsTransient || TransientSqlServerErrors.Contains(sqlEx.Number),
            NpgsqlException npgEx => npgEx.IsTransient || IsTransientNpgSqlState(npgEx.SqlState),
            TimeoutException => true,
            SocketException => true,
            _ => false
        };

    private static bool IsTransientNpgSqlState(string? sqlState) =>
        sqlState is not null && (
            sqlState.StartsWith(PgConnectionExceptionPrefix, StringComparison.Ordinal) ||
            sqlState == PgSqlStateSerializationFailure ||
            sqlState == PgSqlStateAdminShutdown);
}
