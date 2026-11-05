namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Case entity.
/// </summary>
public record Case
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string CaseNumber { get; init; } = string.Empty;
    public string? ContactId { get; init; }
    public string? AccountId { get; init; }
    public string? ParentId { get; init; }
    public string? SuppliedName { get; init; }
    public string? SuppliedEmail { get; init; }
    public string? SuppliedPhone { get; init; }
    public string? SuppliedCompany { get; init; }
    public string? Type { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Reason { get; init; }
    public string? Origin { get; init; }
    public string? Subject { get; init; }
    public string Priority { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsClosed { get; init; }
    public DateTime? ClosedDate { get; init; }
    public bool IsEscalated { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
