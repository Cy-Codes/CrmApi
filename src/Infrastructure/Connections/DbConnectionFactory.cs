using System.Collections.Immutable;
using System.Data.Common;
using Domain.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Connections;

/// <summary>
/// Implementation of IDbConnectionFactory resolving connection strings from configuration.
/// </summary>
public sealed class DbConnectionFactory : IDbConnectionFactory
{
    public const string DefaultMsSqlConnectionString =
        "Server=localhost,1433;Database=CRMDatabaseMS;User Id=sa;Password=CrmDev2026!Secure#Pass;TrustServerCertificate=True;";

    public const string DefaultPgSqlConnectionString =
        "Host=localhost;Port=5432;Database=CRMDatabase;Username=postgres;Password=postgres;";

    private static readonly ImmutableArray<string> MsSqlCandidateKeys =
        ["CRMDatabaseMS", "ms", "MsSql"];

    private static readonly ImmutableArray<string> PgSqlCandidateKeys =
        ["CRMDatabasePG", "pg", "PgSql"];

    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public DbConnection CreateConnection(DatabaseProvider provider) =>
        provider switch
        {
            DatabaseProvider.Ms => new SqlConnection(
                ResolveConnectionString(MsSqlCandidateKeys, DefaultMsSqlConnectionString)),
            DatabaseProvider.Pg => new NpgsqlConnection(
                ResolveConnectionString(PgSqlCandidateKeys, DefaultPgSqlConnectionString)),
            _ => throw new ArgumentOutOfRangeException(
                nameof(provider),
                provider,
                $"Unsupported database provider: {provider}")
        };

    public void ValidateConfiguration(bool allowFallbackDefaults = false)
    {
        if (allowFallbackDefaults)
        {
            return;
        }

        var msConfigured = TryGetRawConnectionString(MsSqlCandidateKeys) is not null;
        var pgConfigured = TryGetRawConnectionString(PgSqlCandidateKeys) is not null;

        if (!msConfigured || !pgConfigured)
        {
            var missing = new List<string>();
            if (!msConfigured) missing.Add("SQL Server (CRMDatabaseMS)");
            if (!pgConfigured) missing.Add("PostgreSQL (CRMDatabasePG)");

            throw new InvalidOperationException(
                $"Missing required database connection string configuration for: {string.Join(", ", missing)}. " +
                "Connection strings must be configured explicitly when outside development fallback mode.");
        }
    }

    private string? TryGetRawConnectionString(ImmutableArray<string> candidateKeys) =>
        candidateKeys
            .Select(key => _configuration.GetConnectionString(key) ?? _configuration[$"ConnectionStrings:{key}"])
            .FirstOrDefault(static value => !string.IsNullOrWhiteSpace(value));

    private string ResolveConnectionString(ImmutableArray<string> candidateKeys, string defaultConnectionString) =>
        TryGetRawConnectionString(candidateKeys) ?? defaultConnectionString;
}
