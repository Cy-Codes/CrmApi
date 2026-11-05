namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot ObjectAssociation entity.
/// </summary>
public record ObjectAssociation
{
    public long AssociationId { get; init; }
    public string FromObjectType { get; init; } = string.Empty;
    public long FromObjectId { get; init; }
    public string ToObjectType { get; init; } = string.Empty;
    public long ToObjectId { get; init; }
    public int AssociationTypeId { get; init; }
    public DateTime CreatedAt { get; init; }
}
