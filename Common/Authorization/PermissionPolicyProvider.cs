using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LUPA.Api.Common.Authorization;

/// <summary>
/// Resuelve políticas de autorización "sobre la marcha" a partir del nombre de policy
/// (ej. "Permission:USER_DELETE"), sin necesidad de registrar cada permiso individualmente
/// en Program.cs. Cualquier policy que no empiece con el prefijo "Permission:" se delega
/// al proveedor por defecto de ASP.NET Core (ej. políticas normales que sí quieras declarar a mano).
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(HasPermissionAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            string permission = policyName[HasPermissionAttribute.PolicyPrefix.Length..];

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}