namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 BusinessUnit entity.
/// </summary>
public record BusinessUnit
{
    public Guid BusinessUnitId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? ParentBusinessUnitId { get; init; }
    public bool IsDisabled { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime ModifiedOn { get; init; }
}
