namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 SystemUser entity.
/// </summary>
public record SystemUser
{
    public Guid SystemUserId { get; init; }
    public Guid BusinessUnitId { get; init; }
    public string DomainName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string? InternalEmailAddress { get; init; }
    public string? JobTitle { get; init; }
    public bool IsDisabled { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime ModifiedOn { get; init; }
}
