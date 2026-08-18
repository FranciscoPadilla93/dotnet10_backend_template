using Microsoft.AspNetCore.Authorization;

namespace LUPA.Api.Common.Authorization;

/// <summary>
/// Valida el PermissionRequirement contra los claims "permission" que vienen dentro del JWT
/// (sellados ahí desde el login/refresh, sin necesidad de consultar la base de datos por petición).
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        bool hasPermission = context.User.HasClaim(
            claim => claim.Type == PermissionClaimTypes.Permission
                     && claim.Value == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}