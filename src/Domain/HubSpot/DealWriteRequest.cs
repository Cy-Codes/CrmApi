using System.ComponentModel.DataAnnotations;

namespace Domain.HubSpot;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a HubSpot Deal.
/// Optional <c>Archived</c> left <c>null</c> keeps the database default (create) or current value (update).
/// </summary>
public record DealWriteRequest
{
    [Required, StringLength(255, MinimumLength = 1)] public string DealName { get; init; } = string.Empty;
    [Required, StringLength(50, MinimumLength = 1)] public string PipelineId { get; init; } = string.Empty;
    [Required, StringLength(50, MinimumLength = 1)] public string DealStageId { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public DateTime? Closedate { get; init; }
    [StringLength(50)] public string? DealType { get; init; }
    public long? OwnerId { get; init; }
    public bool? Archived { get; init; }
}
