namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Pipeline entity.
/// </summary>
public record Pipeline
{
    public string PipelineId { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public int DisplayOrder { get; init; }
    public string ObjectType { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
