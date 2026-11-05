namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce OpportunityLineItem entity.
/// </summary>
public record OpportunityLineItem
{
    public string Id { get; init; } = string.Empty;
    public string OpportunityId { get; init; } = string.Empty;
    public string PricebookEntryId { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal? TotalPrice { get; init; }
    public decimal? ListPrice { get; init; }
    public string? Description { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
