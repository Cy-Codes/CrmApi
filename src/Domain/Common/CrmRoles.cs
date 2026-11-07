namespace Domain.Common;

/// <summary>
/// Role names carried in the JWT <c>roles</c> claim. Anonymous callers hold no role and are read-only.
/// </summary>
public static class CrmRoles
{
    /// <summary>JWT claim type carrying the caller's roles (single value or array).</summary>
    public const string ClaimType = "roles";

    /// <summary>Full control: read, create, update, and delete.</summary>
    public const string Admin = "admin";

    /// <summary>Authenticated user: read, create, and update (no delete).</summary>
    public const string User = "user";

    /// <summary>Authorization policy name allowing create and update (roles <see cref="Admin"/> or <see cref="User"/>).</summary>
    public const string WritePolicy = "CrmWrite";

    /// <summary>Authorization policy name allowing delete (role <see cref="Admin"/> only).</summary>
    public const string DeletePolicy = "CrmDelete";
}
