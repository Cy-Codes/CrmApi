using System.Text.Json;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private const string ProblemJsonContentType = "application/problem+json";
    private const string BadRequestTitle = "Bad Request";
    private const string NotFoundTitle = "Not Found";
    private const string ConflictTitle = "Conflict";
    private const string InternalServerErrorTitle = "Internal Server Error";
    private const string GenericInternalErrorDetail = "An unexpected server error occurred.";

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during request execution: {Message}", ex.Message);
            var problemDetails = CreateProblemDetails(context.Request.Path, ex, _environment.IsDevelopment());
            await WriteProblemResponseAsync(context, problemDetails);
        }
    }

    /// <summary>
    /// Pure mapping function creating an RFC 7807 <see cref="ProblemDetails"/> envelope from an exception.
    /// </summary>
    public static ProblemDetails CreateProblemDetails(
        string requestPath,
        Exception exception,
        bool includeInternalErrorDetails)
    {
        var statusCode = exception switch
        {
            ArgumentException or BadHttpRequestException => StatusCodes.Status400BadRequest,
            System.Collections.Generic.KeyNotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = statusCode switch
        {
            StatusCodes.Status400BadRequest => BadRequestTitle,
            StatusCodes.Status404NotFound => NotFoundTitle,
            StatusCodes.Status409Conflict => ConflictTitle,
            _ => InternalServerErrorTitle
        };

        var detail = statusCode == StatusCodes.Status500InternalServerError && !includeInternalErrorDetails
            ? GenericInternalErrorDetail
            : exception.Message;

        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = requestPath
        };
    }

    /// <summary>
    /// Side-effecting I/O boundary writing the serialized <see cref="ProblemDetails"/> payload to the HTTP response stream.
    /// </summary>
    private static Task WriteProblemResponseAsync(HttpContext context, ProblemDetails problemDetails)
    {
        context.Response.ContentType = ProblemJsonContentType;
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        var json = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(json);
    }
}
