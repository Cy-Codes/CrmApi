using Api.Extensions;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public abstract class BaseCrmController : ControllerBase
{
    protected static DatabaseProvider ParseProvider(string provider) =>
        provider.ToDatabaseProvider();

    protected QueryOptions CreateQueryOptions(
        int page = QueryOptions.DefaultPage,
        int pageSize = QueryOptions.DefaultPageSize,
        string? sortBy = null,
        string sortDirection = QueryOptions.DefaultSortDirection) =>
        Request.Query.ToQueryOptions(page, pageSize, sortBy, sortDirection);
}
