using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Dynamics365;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a Dynamics 365 Opportunity.
/// Optional <c>StateCode</c> / <c>StatusCode</c> left <c>null</c> keep the database default (create) or current value (update).
/// </summary>
public record OpportunityWriteRequest
{
    [Required, StringLength(300, MinimumLength = 1)] public string Name { get; init; } = string.Empty;
    public Guid? ParentAccountId { get; init; }
    public Guid? Parentcontactid { get; init; }
    public Guid? PriceLevelId { get; init; }
    public Guid? OriginatingLeadId { get; init; }
    public decimal? EstimatedValue { get; init; }
    public decimal? ActualValue { get; init; }
    public DateTime? EstimatedCloseDate { get; init; }
    public DateTime? ActualCloseDate { get; init; }
    public int? CloseProbability { get; init; }
    [StringLength(200)] public string? Stepname { get; init; }
    public int? Salesstage { get; init; }
    public string? Description { get; init; }
    [NotEmptyGuid] public Guid OwnerId { get; init; }
    [NotEmptyGuid] public Guid Owningbusinessunit { get; init; }
    public int? StateCode { get; init; }
    public int? StatusCode { get; init; }
}
