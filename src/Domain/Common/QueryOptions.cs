namespace Domain.Common;

/// <summary>
/// Immutable query options for pagination, sorting, and field-level filtering.
/// </summary>
public record QueryOptions(
    int Page = QueryOptions.DefaultPage,
    int PageSize = QueryOptions.DefaultPageSize,
    string? SortBy = null,
    string SortDirection = QueryOptions.DefaultSortDirection,
    IReadOnlyDictionary<string, string?>? Filters = null
)
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPage = 100_000;
    public const int MaxPageSize = 100;
    public const string DefaultSortDirection = "asc";
    public const string DescendingSortDirection = "desc";
    public const string DefaultIdColumn = "Id";

    /// <summary>
    /// Shared immutable default query options instance.
    /// </summary>
    public static readonly QueryOptions Default = new();

    /// <summary>
    /// Pure calculation returning a valid 1-based page number clamped to [DefaultPage, MaxPage].
    /// </summary>
    public int NormalizedPage => Page < DefaultPage ? DefaultPage : (Page > MaxPage ? MaxPage : Page);

    /// <summary>
    /// Pure calculation clamping PageSize within [1, MaxPageSize].
    /// </summary>
    public int NormalizedPageSize => PageSize is < DefaultPage or > MaxPageSize ? DefaultPageSize : PageSize;
}
