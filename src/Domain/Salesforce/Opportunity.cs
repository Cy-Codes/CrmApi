namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Opportunity entity.
/// </summary>
public record Opportunity
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string? AccountId { get; init; }
    public bool IsPrivate { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string StageName { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public decimal? Probability { get; init; }
    public decimal? ExpectedRevenue { get; init; }
    public DateTime CloseDate { get; init; }
    public string? Type { get; init; }
    public string? LeadSource { get; init; }
    public bool IsClosed { get; init; }
    public bool IsWon { get; init; }
    public string ForecastCategoryName { get; init; } = string.Empty;
    public string? CampaignId { get; init; }
    public string? Pricebook2Id { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
