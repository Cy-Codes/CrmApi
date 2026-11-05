namespace Domain.HubSpot;

/// <summary>
/// Domain model for HubSpot Contact entity.
/// </summary>
public record Contact
{
    public long ContactId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Phone { get; init; }
    public string? MobilePhone { get; init; }
    public string? JobTitle { get; init; }
    public long? AssociatedCompanyId { get; init; }
    public long? OwnerId { get; init; }
    public string Lifecyclestage { get; init; } = string.Empty;
    public string? LeadStatus { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Zip { get; init; }
    public string? Country { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool Archived { get; init; }
}
