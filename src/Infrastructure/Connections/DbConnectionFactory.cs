using System.Data.Common;
using Domain.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Connections;

/// <summary>
/// Implementation of IDbConnectionFactory resolving connection strings from configuration.
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public DbConnection CreateConnection(DatabaseProvider provider)
    {
        return provider switch
        {
            DatabaseProvider.Ms => CreateMsSqlConnection(),
            DatabaseProvider.Pg => CreatePgSqlConnection(),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), $"Unsupported database provider: {provider}")
        };
    }

    private SqlConnection CreateMsSqlConnection()
    {
        var connStr = _configuration.GetConnectionString("CRMDatabaseMS")
                      ?? _configuration.GetConnectionString("ms")
                      ?? _configuration.GetConnectionString("MsSql")
                      ?? _configuration["ConnectionStrings:CRMDatabaseMS"]
                      ?? "Server=localhost,1433;Database=CRMDatabaseMS;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";

        return new SqlConnection(connStr);
    }

    private NpgsqlConnection CreatePgSqlConnection()
    {
        var connStr = _configuration.GetConnectionString("CRMDatabasePG")
                      ?? _configuration.GetConnectionString("pg")
                      ?? _configuration.GetConnectionString("PgSql")
                      ?? _configuration["ConnectionStrings:CRMDatabasePG"]
                      ?? "Host=localhost;Port=5432;Database=CRMDatabasePG;Username=postgres;Password=postgres;";

        return new NpgsqlConnection(connStr);
    }
}
