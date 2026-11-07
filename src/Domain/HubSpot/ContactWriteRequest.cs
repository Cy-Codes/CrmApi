using System.ComponentModel.DataAnnotations;

namespace Domain.HubSpot;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a HubSpot Contact.
/// Optional <c>Lifecyclestage</c> / <c>Archived</c> left <c>null</c> keep the database default (create) or current value (update).
/// </summary>
public record ContactWriteRequest
{
    [Required, EmailAddress, StringLength(255, MinimumLength = 1)] public string Email { get; init; } = string.Empty;
    [StringLength(100)] public string? FirstName { get; init; }
    [StringLength(100)] public string? LastName { get; init; }
    [StringLength(50)] public string? Phone { get; init; }
    [StringLength(50)] public string? MobilePhone { get; init; }
    [StringLength(100)] public string? JobTitle { get; init; }
    public long? AssociatedCompanyId { get; init; }
    public long? OwnerId { get; init; }
    [StringLength(50)] public string? Lifecyclestage { get; init; }
    [StringLength(50)] public string? LeadStatus { get; init; }
    [StringLength(255)] public string? Address { get; init; }
    [StringLength(100)] public string? City { get; init; }
    [StringLength(100)] public string? State { get; init; }
    [StringLength(30)] public string? Zip { get; init; }
    [StringLength(100)] public string? Country { get; init; }
    public bool? Archived { get; init; }
}
