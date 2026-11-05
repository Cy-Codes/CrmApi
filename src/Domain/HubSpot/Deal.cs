namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Deal entity.
/// </summary>
public record Deal
{
    public long DealId { get; init; }
    public string DealName { get; init; } = string.Empty;
    public string PipelineId { get; init; } = string.Empty;
    public string DealStageId { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public DateTime? Closedate { get; init; }
    public string? DealType { get; init; }
    public long? OwnerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
