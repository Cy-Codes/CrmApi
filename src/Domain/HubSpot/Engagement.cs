namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Engagement entity.
/// </summary>
public record Engagement
{
    public long EngagementId { get; init; }
    public string EngagementType { get; init; } = string.Empty;
    public long? OwnerId { get; init; }
    public DateTime Timestamp { get; init; }
    public string? Body { get; init; }
    public string? Status { get; init; }
    public long? ActivityDurationMilliseconds { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
