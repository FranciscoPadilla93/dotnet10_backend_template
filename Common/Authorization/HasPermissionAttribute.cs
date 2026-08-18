using Microsoft.AspNetCore.Authorization;

namespace LUPA.Api.Common.Authorization;

/// <summary>
/// Uso: [HasPermission("USER_DELETE")] sobre un controller o action.
/// Internamente arma el nombre de policy "Permission:USER_DELETE", que PermissionPolicyProvider
/// resuelve dinámicamente en tiempo de ejecución sin necesidad de registrar cada permiso a mano.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permission:";

    public HasPermissionAttribute(string permission)
        : base(policy: $"{PolicyPrefix}{permission}")
    {
    }
}