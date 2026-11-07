using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Dynamics365;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a Dynamics 365 Contact.
/// <c>FullName</c> is derived server-side from <c>FirstName</c> and <c>LastName</c>.
/// Optional <c>StateCode</c> / <c>StatusCode</c> left <c>null</c> keep the database default (create) or current value (update).
/// </summary>
public record ContactWriteRequest
{
    public Guid? ParentCustomerId { get; init; }
    [StringLength(50)] public string? FirstName { get; init; }
    [Required, StringLength(50, MinimumLength = 1)] public string LastName { get; init; } = string.Empty;
    [StringLength(100)] public string? JobTitle { get; init; }
    [StringLength(100)] public string? EmailAddress1 { get; init; }
    [StringLength(50)] public string? Telephone1 { get; init; }
    [StringLength(50)] public string? Mobilephone { get; init; }
    [StringLength(250)] public string? Address1_Line1 { get; init; }
    [StringLength(80)] public string? Address1_City { get; init; }
    [StringLength(50)] public string? Address1_StateOrProvince { get; init; }
    [StringLength(20)] public string? Address1_PostalCode { get; init; }
    [StringLength(80)] public string? Address1_Country { get; init; }
    public int? Leadsourcecode { get; init; }
    public string? Description { get; init; }
    [NotEmptyGuid] public Guid OwnerId { get; init; }
    [NotEmptyGuid] public Guid Owningbusinessunit { get; init; }
    public int? StateCode { get; init; }
    public int? StatusCode { get; init; }
}
