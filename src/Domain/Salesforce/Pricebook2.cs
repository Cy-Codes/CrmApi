namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Pricebook2 entity.
/// </summary>
public record Pricebook2
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public bool IsStandard { get; init; }
    public string? Description { get; init; }
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
