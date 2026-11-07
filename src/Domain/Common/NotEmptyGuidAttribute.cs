using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

/// <summary>
/// Validation attribute rejecting <see cref="Guid.Empty"/> for required GUID request fields.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEmptyGuidAttribute : ValidationAttribute
{
    public NotEmptyGuidAttribute()
        : base("The {0} field must be a non-empty GUID.")
    {
    }

    public override bool IsValid(object? value) =>
        value is Guid guid && guid != Guid.Empty;
}
