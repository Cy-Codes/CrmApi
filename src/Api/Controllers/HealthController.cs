using Domain.Common;
using Infrastructure.Connections;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("health")]
public sealed class HealthController : ControllerBase
{
    private const string HealthyStatus = "Healthy";
    private const string ConnectedStatus = "Connected";
    private const string UnhealthyStatus = "Unhealthy";

    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        IDbConnectionFactory connectionFactory,
        ILogger<HealthController> logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public IActionResult Get() => Ok(new { status = HealthyStatus, timestamp = DateTime.UtcNow });

    [HttpGet(DatabaseProviderExtensions.MsProviderCode)]
    public Task<IActionResult> CheckMsSql(CancellationToken ct) =>
        ExecuteDatabaseHealthCheckAsync(DatabaseProvider.Ms, "SQL Server", ct);

    [HttpGet(DatabaseProviderExtensions.PgProviderCode)]
    public Task<IActionResult> CheckPgSql(CancellationToken ct) =>
        ExecuteDatabaseHealthCheckAsync(DatabaseProvider.Pg, "PostgreSQL", ct);

    /// <summary>
    /// Side-effecting I/O helper executing a live database connectivity probe.
    /// </summary>
    private async Task<IActionResult> ExecuteDatabaseHealthCheckAsync(
        DatabaseProvider provider,
        string engineDisplayName,
        CancellationToken ct)
    {
        var providerCode = provider.ToProviderCode();
        try
        {
            await using var conn = _connectionFactory.CreateConnection(provider);
            await conn.OpenAsync(ct);
            return Ok(new { provider = providerCode, status = ConnectedStatus, server = conn.Database });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "{Engine} health check failed: {Message}", engineDisplayName, ex.Message);
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new { provider = providerCode, status = UnhealthyStatus, error = ex.Message });
        }
    }
}
