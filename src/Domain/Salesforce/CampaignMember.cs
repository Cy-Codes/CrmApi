namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce CampaignMember entity.
/// </summary>
public record CampaignMember
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string CampaignId { get; init; } = string.Empty;
    public string? LeadId { get; init; }
    public string? ContactId { get; init; }
    public string? Status { get; init; }
    public bool HasResponded { get; init; }
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
