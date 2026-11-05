using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public abstract class BaseCrmController : ControllerBase
{
    protected DatabaseProvider ParseProvider(string provider)
    {
        return provider.ToLowerInvariant() switch
        {
            "ms" => DatabaseProvider.Ms,
            "pg" => DatabaseProvider.Pg,
            _ => throw new ArgumentException($"Invalid provider '{provider}'. Must be 'ms' or 'pg'.", nameof(provider))
        };
    }

    protected QueryOptions CreateQueryOptions(
        int page = 1,
        int pageSize = 20,
        string? sortBy = null,
        string sortDirection = "asc")
    {
        var filters = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (var queryParam in Request.Query)
        {
            var key = queryParam.Key;
            if (string.Equals(key, "page", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(key, "pageSize", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(key, "sortBy", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(key, "sortDirection", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            filters[key] = queryParam.Value.ToString();
        }

        return new QueryOptions(page, pageSize, sortBy, sortDirection, filters);
    }
}
