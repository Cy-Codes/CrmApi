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
}
