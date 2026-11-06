namespace Domain.Common;

/// <summary>
/// Pure extension functions for parsing and formatting <see cref="DatabaseProvider"/> values.
/// </summary>
public static class DatabaseProviderExtensions
{
    public const string MsProviderCode = "ms";
    public const string PgProviderCode = "pg";

    /// <summary>
    /// Pure transformation parsing a route provider string ('ms' or 'pg') into a <see cref="DatabaseProvider"/>.
    /// </summary>
    public static DatabaseProvider ToDatabaseProvider(this string provider) =>
        provider?.ToLowerInvariant() switch
        {
            MsProviderCode => DatabaseProvider.Ms,
            PgProviderCode => DatabaseProvider.Pg,
            _ => throw new ArgumentException(
                $"Invalid provider '{provider}'. Must be '{MsProviderCode}' or '{PgProviderCode}'.",
                nameof(provider))
        };

    /// <summary>
    /// Pure transformation formatting a <see cref="DatabaseProvider"/> into its canonical route code.
    /// </summary>
    public static string ToProviderCode(this DatabaseProvider provider) =>
        provider switch
        {
            DatabaseProvider.Ms => MsProviderCode,
            DatabaseProvider.Pg => PgProviderCode,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, "Unsupported database provider.")
        };
}
