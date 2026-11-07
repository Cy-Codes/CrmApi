using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api.Middleware;

/// <summary>
/// Dynamic authorization policy provider that enforces JWT Bearer authentication on all endpoints
/// when Authentication:Enabled is true, while permitting anonymous access during local development or when disabled.
/// </summary>
public sealed class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private const string AuthEnabledKey = "Authentication:Enabled";
    private readonly IConfiguration _configuration;
    private readonly DefaultAuthorizationPolicyProvider _fallbackProvider;

    public DynamicAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options,
        IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _fallbackProvider = new DefaultAuthorizationPolicyProvider(options ?? throw new ArgumentNullException(nameof(options)));
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        _fallbackProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName) =>
        _fallbackProvider.GetPolicyAsync(policyName);

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        var authEnabled = _configuration.GetValue(AuthEnabledKey, false);
        if (authEnabled)
        {
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return Task.FromResult<AuthorizationPolicy?>(null);
    }
}
