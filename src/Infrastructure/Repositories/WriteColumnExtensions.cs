using System.Collections.Immutable;

namespace Infrastructure.Repositories;

/// <summary>
/// Pure helpers composing immutable column/value sets for INSERT and UPDATE commands.
/// </summary>
public static class WriteColumnExtensions
{
    /// <summary>Empty immutable column set.</summary>
    public static readonly ImmutableDictionary<string, object?> Empty = ImmutableDictionary<string, object?>.Empty;

    /// <summary>Adds (or replaces) a column, writing <c>NULL</c> when <paramref name="value"/> is null.</summary>
    public static ImmutableDictionary<string, object?> With(
        this ImmutableDictionary<string, object?> columns,
        string name,
        object? value) =>
        columns.SetItem(name, value);

    /// <summary>Adds a column only when <paramref name="value"/> is non-null, so database defaults / current values are preserved.</summary>
    public static ImmutableDictionary<string, object?> WithOptional(
        this ImmutableDictionary<string, object?> columns,
        string name,
        object? value) =>
        value is null ? columns : columns.SetItem(name, value);

    /// <summary>Normalizes a timestamp to UTC (required by PostgreSQL <c>timestamptz</c> parameters).</summary>
    public static DateTime AsUtc(this DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };

    /// <summary>Normalizes an optional timestamp to UTC.</summary>
    public static DateTime? AsUtc(this DateTime? value) => value?.AsUtc();

    /// <summary>Drops the time portion for <c>DATE</c> columns.</summary>
    public static DateTime? AsDate(this DateTime? value) => value?.Date;
}
