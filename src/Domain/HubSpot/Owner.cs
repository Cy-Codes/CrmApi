namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Owner entity.
/// </summary>
public record Owner
{
    public long OwnerId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public long? UserId { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
