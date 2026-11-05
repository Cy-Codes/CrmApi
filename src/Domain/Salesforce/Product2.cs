namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Product2 entity.
/// </summary>
public record Product2
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? ProductCode { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public string? Family { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
