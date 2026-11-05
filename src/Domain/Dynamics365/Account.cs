namespace Domain.Dynamics365;

/// <summary>
/// Domain model for Dynamics365 Account entity.
/// </summary>
public record Account
{
    public Guid AccountId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? AccountNumber { get; init; }
    public Guid? ParentAccountId { get; init; }
    public string? Telephone1 { get; init; }
    public string? EmailAddress1 { get; init; }
    public string? Websiteurl { get; init; }
    public string? Address1_Line1 { get; init; }
    public string? Address1_City { get; init; }
    public string? Address1_StateOrProvince { get; init; }
    public string? Address1_PostalCode { get; init; }
    public string? Address1_Country { get; init; }
    public decimal? Revenue { get; init; }
    public int? NumberOfEmployees { get; init; }
    public int? Industrycode { get; init; }
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
