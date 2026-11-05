namespace Domain.Salesforce;

/// <summary>
/// Domain model for Salesforce Account entity.
/// </summary>
public record Account
{
    public string Id { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string? ParentId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Type { get; init; }
    public string? BillingStreet { get; init; }
    public string? BillingCity { get; init; }
    public string? BillingState { get; init; }
    public string? BillingPostalCode { get; init; }
    public string? BillingCountry { get; init; }
    public string? ShippingStreet { get; init; }
    public string? ShippingCity { get; init; }
    public string? ShippingState { get; init; }
    public string? ShippingPostalCode { get; init; }
    public string? ShippingCountry { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    public string? Website { get; init; }
    public string? Industry { get; init; }
    public decimal? AnnualRevenue { get; init; }
    public int? NumberOfEmployees { get; init; }
    public string? Description { get; init; }
    public string OwnerId { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public string CreatedById { get; init; } = string.Empty;
    public DateTime LastModifiedDate { get; init; }
    public string LastModifiedById { get; init; } = string.Empty;
}
