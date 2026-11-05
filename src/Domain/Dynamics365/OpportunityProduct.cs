namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 OpportunityProduct entity.
/// </summary>
public record OpportunityProduct
{
    public Guid OpportunityProductId { get; init; }
    public Guid OpportunityId { get; init; }
    public Guid? ProductId { get; init; }
    public string? ProductDescription { get; init; }
    public decimal PricePerUnit { get; init; }
    public decimal Quantity { get; init; }
    public decimal? ExtendedAmount { get; init; }
    public decimal ManualDiscountAmount { get; init; }
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    public Guid ModifiedBy { get; init; }
}
