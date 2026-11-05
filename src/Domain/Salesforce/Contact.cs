namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Contact entity.
/// </summary>
public record Contact
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string? AccountId { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string? Salutation { get; init; }
    public string? Name { get; init; }
    public string? ReportsToId { get; init; }
    public string? Title { get; init; }
    public string? Department { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? MobilePhone { get; init; }
    public string? MailingStreet { get; init; }
    public string? MailingCity { get; init; }
    public string? MailingState { get; init; }
    public string? MailingPostalCode { get; init; }
    public string? MailingCountry { get; init; }
    public string? LeadSource { get; init; }
    public string? Description { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
