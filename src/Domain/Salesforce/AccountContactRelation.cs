namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce AccountContactRelation entity.
/// </summary>
public record AccountContactRelation
{
    public string Id { get; init; } = string.Empty;
    public string AccountId { get; init; } = string.Empty;
    public string ContactId { get; init; } = string.Empty;
    public string? Roles { get; init; }
    public bool IsDirect { get; init; }
    public bool IsActive { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime LastModifiedDate { get; init; }
}
