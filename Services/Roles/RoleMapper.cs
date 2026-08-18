using LUPA.Api.Entities;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Roles;

public static class RoleMapper
{
    public static RoleResponse ToResponse(
        Role role,
        IReadOnlyCollection<string> permissions)
    {
        return new RoleResponse
        {
            Id = role.Id,
            Code = role.Code,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive,
            Permissions = permissions
        };
    }
}