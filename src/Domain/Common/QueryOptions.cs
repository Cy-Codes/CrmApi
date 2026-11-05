namespace Domain.Common;

/// <summary>
/// Query options for pagination, sorting, and field-level filtering.
/// </summary>
public record QueryOptions(
    int Page = 1,
    int PageSize = 20,
    string? SortBy = null,
    string SortDirection = "asc",
    Dictionary<string, string?>? Filters = null
)
{
    /// <summary>
    /// Default query options.
    /// </summary>
    public static QueryOptions Default => new(1, 20, null, "asc", null);
}
