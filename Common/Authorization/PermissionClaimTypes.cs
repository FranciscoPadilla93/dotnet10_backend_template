namespace LUPA.Api.Common.Authorization;

/// <summary>
/// Nombres de claim custom usados por el sistema de autorización basado en permisos.
/// Centralizado aquí para que JwtService (quien los genera) y PermissionAuthorizationHandler
/// (quien los valida) siempre usen exactamente el mismo string.
/// </summary>
public static class PermissionClaimTypes
{
    public const string Permission = "permission";
}