namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Company entity.
/// </summary>
public record Company
{
    public long CompanyId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Domain { get; init; }
    public string? Industry { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Zip { get; init; }
    public string? Country { get; init; }
    public decimal? AnnualRevenue { get; init; }
    public int? NumberOfEmployees { get; init; }
    public string? Description { get; init; }
    public long? OwnerId { get; init; }
    public string Lifecyclestage { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
