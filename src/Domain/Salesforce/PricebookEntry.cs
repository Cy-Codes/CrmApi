namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce PricebookEntry entity.
/// </summary>
public record PricebookEntry
{
    public string Id { get; init; } = string.Empty;
    public string Pricebook2Id { get; init; } = string.Empty;
    public string Product2Id { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public bool IsActive { get; init; }
    public bool UseStandardPrice { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
