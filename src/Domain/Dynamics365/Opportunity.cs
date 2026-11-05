namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 Opportunity entity.
/// </summary>
public record Opportunity
{
    public Guid OpportunityId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid? ParentAccountId { get; init; }
    public Guid? Parentcontactid { get; init; }
    public Guid? PriceLevelId { get; init; }
    public Guid? OriginatingLeadId { get; init; }
    public decimal? EstimatedValue { get; init; }
    public decimal? ActualValue { get; init; }
    public DateTime? EstimatedCloseDate { get; init; }
    public DateTime? ActualCloseDate { get; init; }
    public int? CloseProbability { get; init; }
    public string? Stepname { get; init; }
    public int? Salesstage { get; init; }
    public string? Description { get; init; }
    public Guid OwnerId { get; init; }
    public Guid Owningbusinessunit { get; init; }
    public int StateCode { get; init; }
    public int StatusCode { get; init; }
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    public Guid ModifiedBy { get; init; }
}
