namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot DealStage entity.
/// </summary>
public record DealStage
{
    public string StageId { get; init; } = string.Empty;
    public string PipelineId { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public int DisplayOrder { get; init; }
    public decimal Probability { get; init; }
    public bool ClosedWon { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
