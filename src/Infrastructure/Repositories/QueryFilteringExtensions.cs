using System.Globalization;
using System.Text.RegularExpressions;
using Domain.Common;
using SqlKata;

namespace Infrastructure.Repositories;

/// <summary>
/// Pure functional extension methods for composing SqlKata <see cref="Query"/> transformations.
/// </summary>
public static partial class QueryFilteringExtensions
{
    private const char SqlWildcardChar = '%';

    [GeneratedRegex("^[a-zA-Z0-9_.]+$")]
    private static partial Regex SafeIdentifierRegex();

    /// <summary>
    /// Pure check validating whether a column or table identifier is safe against SQL injection.
    /// </summary>
    public static bool IsSafeSqlIdentifier(this string? identifier) =>
        !string.IsNullOrWhiteSpace(identifier) && SafeIdentifierRegex().IsMatch(identifier);

    /// <summary>
    /// Pure culture-invariant parser converting a raw query filter string into its strongly-typed scalar value.
    /// </summary>
    public static object ParseFilterValue(this string rawValue) =>
        rawValue switch
        {
            _ when bool.TryParse(rawValue, out var boolVal) => boolVal,
            _ when int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intVal) => intVal,
            _ when long.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longVal) => longVal,
            _ when decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var decVal) => decVal,
            _ when Guid.TryParse(rawValue, out var guidVal) => guidVal,
            _ => rawValue
        };

    /// <summary>
    /// Applies valid field-level filters to a <see cref="Query"/> using a declarative functional fold.
    /// </summary>
    public static Query ApplyFilters(this Query query, IReadOnlyDictionary<string, string?>? filters) =>
        filters is null || filters.Count == 0
            ? query
            : filters
                .Where(static kv => kv.Key.IsSafeSqlIdentifier() && !string.IsNullOrWhiteSpace(kv.Value))
                .Aggregate(query, static (currentQuery, kv) => currentQuery.ApplySingleFilter(kv.Key, kv.Value!));

    /// <summary>
    /// Applies a single validated key/value predicate to a <see cref="Query"/>.
    /// </summary>
    public static Query ApplySingleFilter(this Query query, string column, string rawValue) =>
        rawValue.Contains(SqlWildcardChar)
            ? query.WhereLike(column, rawValue)
            : query.Where(column, rawValue.ParseFilterValue());

    /// <summary>
    /// Applies validated sorting order to a <see cref="Query"/>.
    /// </summary>
    public static Query ApplySorting(
        this Query query,
        string? sortBy,
        string sortDirection,
        string defaultSortColumn = QueryOptions.DefaultIdColumn)
    {
        var sortColumn = sortBy.IsSafeSqlIdentifier() ? sortBy! : defaultSortColumn;
        var isDescending = string.Equals(
            sortDirection,
            QueryOptions.DescendingSortDirection,
            StringComparison.OrdinalIgnoreCase);

        return isDescending ? query.OrderByDesc(sortColumn) : query.OrderBy(sortColumn);
    }

    /// <summary>
    /// Applies 1-based page offset and limit to a <see cref="Query"/> with integer overflow protection.
    /// </summary>
    public static Query ApplyPagination(this Query query, int page, int pageSize)
    {
        var safePage = page < QueryOptions.DefaultPage ? QueryOptions.DefaultPage : (page > QueryOptions.MaxPage ? QueryOptions.MaxPage : page);
        var safePageSize = pageSize is < QueryOptions.DefaultPage or > QueryOptions.MaxPageSize ? QueryOptions.DefaultPageSize : pageSize;
        var offset = checked((long)(safePage - 1) * safePageSize);
        return query.Skip(checked((int)offset)).Take(safePageSize);
    }
}
