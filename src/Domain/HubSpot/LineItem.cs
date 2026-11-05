namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot LineItem entity.
/// </summary>
public record LineItem
{
    public long LineItemId { get; init; }
    public long DealId { get; init; }
    public long? ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal? Amount { get; init; }
    public decimal Discount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
