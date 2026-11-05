namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 Product entity.
/// </summary>
public record Product
{
    public Guid ProductId { get; init; }
    public string ProductNumber { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal? Price { get; init; }
    public decimal? StandardCost { get; init; }
    public decimal? CurrentCost { get; init; }
    public decimal QuantityOnHand { get; init; }
    public int StateCode { get; init; }
    public int StatusCode { get; init; }
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    public Guid ModifiedBy { get; init; }
}
