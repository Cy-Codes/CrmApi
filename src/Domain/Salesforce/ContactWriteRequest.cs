using System.ComponentModel.DataAnnotations;

namespace Domain.Salesforce;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a Salesforce Contact.
/// <c>Name</c> is a generated column and is never written.
/// </summary>
public record ContactWriteRequest
{
    [StringLength(18)] public string? AccountId { get; init; }
    [Required, StringLength(80, MinimumLength = 1)] public string LastName { get; init; } = string.Empty;
    [StringLength(40)] public string? FirstName { get; init; }
    [StringLength(40)] public string? Salutation { get; init; }
    [StringLength(18)] public string? ReportsToId { get; init; }
    [StringLength(128)] public string? Title { get; init; }
    [StringLength(80)] public string? Department { get; init; }
    [StringLength(128)] public string? Email { get; init; }
    [StringLength(40)] public string? Phone { get; init; }
    [StringLength(40)] public string? MobilePhone { get; init; }
    [StringLength(255)] public string? MailingStreet { get; init; }
    [StringLength(40)] public string? MailingCity { get; init; }
    [StringLength(80)] public string? MailingState { get; init; }
    [StringLength(20)] public string? MailingPostalCode { get; init; }
    [StringLength(80)] public string? MailingCountry { get; init; }
    [StringLength(40)] public string? LeadSource { get; init; }
    public string? Description { get; init; }
    [Required, StringLength(18, MinimumLength = 1)] public string OwnerId { get; init; } = string.Empty;
}
