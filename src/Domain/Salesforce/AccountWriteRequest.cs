using System.ComponentModel.DataAnnotations;

namespace Domain.Salesforce;

/// <summary>
/// Writable fields for creating (POST) or fully replacing (PUT) a Salesforce Account.
/// Identifier, audit timestamps, and audit users are server-owned.
/// </summary>
public record AccountWriteRequest
{
    [StringLength(18)] public string? ParentId { get; init; }
    [Required, StringLength(255, MinimumLength = 1)] public string Name { get; init; } = string.Empty;
    [StringLength(40)] public string? Type { get; init; }
    [StringLength(255)] public string? BillingStreet { get; init; }
    [StringLength(40)] public string? BillingCity { get; init; }
    [StringLength(80)] public string? BillingState { get; init; }
    [StringLength(20)] public string? BillingPostalCode { get; init; }
    [StringLength(80)] public string? BillingCountry { get; init; }
    [StringLength(255)] public string? ShippingStreet { get; init; }
    [StringLength(40)] public string? ShippingCity { get; init; }
    [StringLength(80)] public string? ShippingState { get; init; }
    [StringLength(20)] public string? ShippingPostalCode { get; init; }
    [StringLength(80)] public string? ShippingCountry { get; init; }
    [StringLength(40)] public string? Phone { get; init; }
    [StringLength(40)] public string? Fax { get; init; }
    [StringLength(255)] public string? Website { get; init; }
    [StringLength(40)] public string? Industry { get; init; }
    public decimal? AnnualRevenue { get; init; }
    public int? NumberOfEmployees { get; init; }
    public string? Description { get; init; }
    [Required, StringLength(18, MinimumLength = 1)] public string OwnerId { get; init; } = string.Empty;
}
