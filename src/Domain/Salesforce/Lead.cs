namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Lead entity.
/// </summary>
public record Lead
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string? Salutation { get; init; }
    public string? Name { get; init; }
    public string? Title { get; init; }
    public string Company { get; init; } = string.Empty;
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public string? Phone { get; init; }
    public string? MobilePhone { get; init; }
    public string? Email { get; init; }
    public string? Website { get; init; }
    public string? LeadSource { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Industry { get; init; }
    public decimal? AnnualRevenue { get; init; }
    public int? NumberOfEmployees { get; init; }
    public string? Description { get; init; }
    public bool IsConverted { get; init; }
    public DateTime? ConvertedDate { get; init; }
    public string? ConvertedAccountId { get; init; }
    public string? ConvertedContactId { get; init; }
    public string? ConvertedOpportunityId { get; init; }
    public bool IsUnreadByOwner { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
