namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 PriceLevel entity.
/// </summary>
public record PriceLevel
{
    public Guid PriceLevelId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime? BeginDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int StateCode { get; init; }
    public int StatusCode { get; init; }
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    public Guid ModifiedBy { get; init; }
}
