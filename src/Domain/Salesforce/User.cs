namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce User entity.
/// </summary>
public record User
{
    public string Id { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public decimal? Name { get; init; }
    public string Email { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public string? UserRoleId { get; init; }
    public string? ProfileId { get; init; }
    public string? Title { get; init; }
    public string? Department { get; init; }
    public string? Phone { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime LastModifiedDate { get; init; }
}
