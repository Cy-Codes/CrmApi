namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Product entity.
/// </summary>
public record Product
{
    public long ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Sku { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public decimal? Cost { get; init; }
    public string? RecurringBillingFrequency { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
