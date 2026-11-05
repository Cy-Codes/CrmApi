using Domain.Common;
using Infrastructure.Connections;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
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
    public IActionResult Get() => Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });

    [HttpGet("ms")]
    public async Task<IActionResult> CheckMsSql(CancellationToken ct)
    {
        try
        {
            await using var conn = _connectionFactory.CreateConnection(DatabaseProvider.Ms);
            await conn.OpenAsync(ct);
            return Ok(new { provider = "ms", status = "Connected", server = conn.Database });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "SQL Server health check failed: {Message}", ex.Message);
            return StatusCode(503, new { provider = "ms", status = "Unhealthy", error = ex.Message });
        }
    }

    [HttpGet("pg")]
    public async Task<IActionResult> CheckPgSql(CancellationToken ct)
    {
        try
        {
            await using var conn = _connectionFactory.CreateConnection(DatabaseProvider.Pg);
            await conn.OpenAsync(ct);
            return Ok(new { provider = "pg", status = "Connected", server = conn.Database });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "PostgreSQL health check failed: {Message}", ex.Message);
            return StatusCode(503, new { provider = "pg", status = "Unhealthy", error = ex.Message });
        }
    }
}
