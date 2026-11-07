using System.ComponentModel.DataAnnotations;

namespace Domain.HubSpot;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a HubSpot Company.
/// Optional <c>Lifecyclestage</c> / <c>Archived</c> left <c>null</c> keep the database default (create) or current value (update).
/// </summary>
public record CompanyWriteRequest
{
    [Required, StringLength(255, MinimumLength = 1)] public string Name { get; init; } = string.Empty;
    [StringLength(255)] public string? Domain { get; init; }
    [StringLength(100)] public string? Industry { get; init; }
    [StringLength(50)] public string? Phone { get; init; }
    [StringLength(255)] public string? Address { get; init; }
    [StringLength(100)] public string? City { get; init; }
    [StringLength(100)] public string? State { get; init; }
    [StringLength(30)] public string? Zip { get; init; }
    [StringLength(100)] public string? Country { get; init; }
    public decimal? AnnualRevenue { get; init; }
    public int? NumberOfEmployees { get; init; }
    public string? Description { get; init; }
    public long? OwnerId { get; init; }
    [StringLength(50)] public string? Lifecyclestage { get; init; }
    public bool? Archived { get; init; }
}
