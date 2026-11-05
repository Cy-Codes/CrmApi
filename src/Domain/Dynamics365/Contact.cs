namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 Contact entity.
/// </summary>
public record Contact
{
    public Guid ContactId { get; init; }
    public Guid? ParentCustomerId { get; init; }
    public string? FirstName { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? JobTitle { get; init; }
    public string? EmailAddress1 { get; init; }
    public string? Telephone1 { get; init; }
    public string? Mobilephone { get; init; }
    public string? Address1_Line1 { get; init; }
    public string? Address1_City { get; init; }
    public string? Address1_StateOrProvince { get; init; }
    public string? Address1_PostalCode { get; init; }
    public string? Address1_Country { get; init; }
    public int? Leadsourcecode { get; init; }
    public string? Description { get; init; }
    public Guid OwnerId { get; init; }
    public Guid Owningbusinessunit { get; init; }
    public int StateCode { get; init; }
    public int StatusCode { get; init; }
    public DateTime CreatedOn { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    public Guid ModifiedBy { get; init; }
}
