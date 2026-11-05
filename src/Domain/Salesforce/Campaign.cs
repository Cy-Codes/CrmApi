namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Campaign entity.
/// </summary>
public record Campaign
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? ParentId { get; init; }
    public string? Type { get; init; }
    public string? Status { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public decimal? ExpectedRevenue { get; init; }
    public decimal? BudgetedCost { get; init; }
    public decimal? ActualCost { get; init; }
    public decimal? ExpectedResponse { get; init; }
    public decimal? NumberSent { get; init; }
    public bool IsActive { get; init; }
    public string? Description { get; init; }
    public int NumberOfLeads { get; init; }
    public int NumberOfConvertedLeads { get; init; }
    public int NumberOfContacts { get; init; }
    public int NumberOfResponses { get; init; }
    public int NumberOfOpportunities { get; init; }
    public int NumberOfWonOpportunities { get; init; }
    public decimal AmountAllOpportunities { get; init; }
    public decimal AmountWonOpportunities { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
