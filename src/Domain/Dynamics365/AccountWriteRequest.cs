using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Dynamics365;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a Dynamics 365 Account.
/// Optional <c>StateCode</c> / <c>StatusCode</c> left <c>null</c> keep the database default (create) or current value (update).
/// </summary>
public record AccountWriteRequest
{
    [Required, StringLength(160, MinimumLength = 1)] public string Name { get; init; } = string.Empty;
    [StringLength(20)] public string? AccountNumber { get; init; }
    public Guid? ParentAccountId { get; init; }
    [StringLength(50)] public string? Telephone1 { get; init; }
    [StringLength(100)] public string? EmailAddress1 { get; init; }
    [StringLength(200)] public string? Websiteurl { get; init; }
    [StringLength(250)] public string? Address1_Line1 { get; init; }
    [StringLength(80)] public string? Address1_City { get; init; }
    [StringLength(50)] public string? Address1_StateOrProvince { get; init; }
    [StringLength(20)] public string? Address1_PostalCode { get; init; }
    [StringLength(80)] public string? Address1_Country { get; init; }
    public decimal? Revenue { get; init; }
    public int? NumberOfEmployees { get; init; }
    public int? Industrycode { get; init; }
    public string? Description { get; init; }
    [NotEmptyGuid] public Guid OwnerId { get; init; }
    [NotEmptyGuid] public Guid Owningbusinessunit { get; init; }
    public int? StateCode { get; init; }
    public int? StatusCode { get; init; }
}
