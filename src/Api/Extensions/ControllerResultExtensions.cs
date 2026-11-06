using System.Collections.Frozen;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

/// <summary>
/// Pure and async extension functions for mapping HTTP query collections and domain results into ASP.NET Core <see cref="ActionResult{TValue}"/> responses.
/// </summary>
public static class ControllerResultExtensions
{
    private static readonly FrozenSet<string> ReservedQueryParameterNames =
        new[] { "page", "pageSize", "sortBy", "sortDirection" }
            .ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Pure transformation converting an <see cref="IQueryCollection"/> and pagination parameters into an immutable <see cref="QueryOptions"/> record.
    /// </summary>
    public static QueryOptions ToQueryOptions(
        this IQueryCollection queryCollection,
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        string? sortBy = null,
        string sortDirection = QueryOptions.DefaultSortDirection)
    {
        var filters = queryCollection
            .Where(static kv => !ReservedQueryParameterNames.Contains(kv.Key))
            .ToFrozenDictionary(
                static kv => kv.Key,
                static kv => (string?)kv.Value.ToString(),
                StringComparer.OrdinalIgnoreCase);

        return new QueryOptions(page, pageSize, sortBy, sortDirection, filters);
    }

    /// <summary>
    /// Pure transformation mapping a nullable entity into <see cref="NotFoundResult"/> (404) or <see cref="OkObjectResult"/> (200).
    /// </summary>
    public static ActionResult<T> ToActionResult<T>(this T? entity)
        where T : class =>
        entity is null ? new NotFoundResult() : new OkObjectResult(entity);

    /// <summary>
    /// Awaits an asynchronous nullable entity query and maps the result into <see cref="NotFoundResult"/> (404) or <see cref="OkObjectResult"/> (200).
    /// </summary>
    public static async Task<ActionResult<T>> ToActionResultAsync<T>(this Task<T?> entityTask)
        where T : class
    {
        var entity = await entityTask;
        return entity.ToActionResult();
    }

    /// <summary>
    /// Awaits an asynchronous non-nullable query and wraps the result in <see cref="OkObjectResult"/> (200).
    /// </summary>
    public static async Task<ActionResult<T>> ToOkActionResultAsync<T>(this Task<T> resultTask)
        where T : class =>
        new OkObjectResult(await resultTask);
}
