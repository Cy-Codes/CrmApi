using System.Data.Common;
using Domain.Common;

namespace Infrastructure.Connections;

/// <summary>
/// Factory for creating database connections based on the specified provider.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates and returns a new DbConnection for the given provider.
    /// </summary>
    /// <param name="provider">Target database engine.</param>
    /// <returns>An unopened DbConnection instance.</returns>
    DbConnection CreateConnection(DatabaseProvider provider);

    /// <summary>
    /// Validates that required database connection strings are present in configuration.
    /// Throws <see cref="InvalidOperationException"/> if required connection strings are missing.
    /// </summary>
    /// <param name="allowFallbackDefaults">Whether fallback localhost defaults are permitted (e.g. in local development).</param>
    void ValidateConfiguration(bool allowFallbackDefaults = false);
}
