using LUPA.Api.Entities;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Permissions;

public static class PermissionMapper
{
    public static PermissionResponse ToResponse(Permission permission)
    {
        return new PermissionResponse
        {
            Id = permission.Id,
            ModuleId = permission.ModuleId,
            Code = permission.Code,
            Name = permission.Name,
            Description = permission.Description
        };
    }
}