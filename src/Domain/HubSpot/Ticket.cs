namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Ticket entity.
/// </summary>
public record Ticket
{
    public long TicketId { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string? Content { get; init; }
    public string PipelineId { get; init; } = string.Empty;
    public string TicketStatusStageId { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public long? OwnerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
