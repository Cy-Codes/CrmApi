namespace Domain.Common;

/// <summary>
/// Supported database backend engines.
/// </summary>
public enum DatabaseProvider
{
    /// <summary>
    /// Microsoft SQL Server (CRMDatabaseMS).
    /// </summary>
    Ms,

    /// <summary>
    /// PostgreSQL (CRMDatabasePG).
    /// </summary>
    Pg
}
