namespace Domain.Common;

/// <summary>
/// Envelope for paginated query results.
/// </summary>
/// <typeparam name="T">Entity model type.</typeparam>
public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize
)
{
    /// <summary>
    /// Total number of available pages.
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>
    /// True if there is a next page available.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// True if there is a previous page available.
    /// </summary>
    public bool HasPreviousPage => Page > 1;
}
