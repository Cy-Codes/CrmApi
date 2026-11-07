using System.ComponentModel.DataAnnotations;

namespace Domain.Salesforce;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a Salesforce Opportunity.
/// <c>ExpectedRevenue</c> is a generated column and is never written. Optional flags left <c>null</c>
/// keep the database default (create) or the current value (update).
/// </summary>
public record OpportunityWriteRequest
{
    [StringLength(18)] public string? AccountId { get; init; }
    public bool? IsPrivate { get; init; }
    [Required, StringLength(120, MinimumLength = 1)] public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    [Required, StringLength(40, MinimumLength = 1)] public string StageName { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    [Range(0, 100)] public decimal? Probability { get; init; }
    [Required] public DateTime? CloseDate { get; init; }
    [StringLength(40)] public string? Type { get; init; }
    [StringLength(40)] public string? LeadSource { get; init; }
    public bool? IsClosed { get; init; }
    public bool? IsWon { get; init; }
    [StringLength(40)] public string? ForecastCategoryName { get; init; }
    [StringLength(18)] public string? CampaignId { get; init; }
    [StringLength(18)] public string? Pricebook2Id { get; init; }
    [Required, StringLength(18, MinimumLength = 1)] public string OwnerId { get; init; } = string.Empty;
}
