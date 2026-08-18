using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Audit;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;

namespace LUPA.Api.Services.Roles;

public class RoleService
    : BaseService<Role, RoleResponse, CreateRoleRequest, UpdateRoleRequest>, IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository, IAuditLogService auditLogService)
        : base(roleRepository, auditLogService)
    {
        _roleRepository = roleRepository;
    }

    protected override string NotFoundMessage => "Rol no encontrado.";

    protected override async Task<RoleResponse> MapToResponseAsync(Role entity)
    {
        var permissions = await _roleRepository.GetPermissionCodesAsync(entity.Id);

        return RoleMapper.ToResponse(entity, permissions);
    }

    protected override async Task<Role> MapToEntityAsync(CreateRoleRequest request)
    {
        bool codeInUse = await _roleRepository.ExistsAsync(x => x.Code == request.Code);

        if (codeInUse)
        {
            throw new ConflictException($"Ya existe un rol con el código '{request.Code}'.");
        }

        return new Role
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };
    }

    protected override Task ApplyUpdateAsync(Role entity, UpdateRoleRequest request)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;

        return Task.CompletedTask;
    }

    protected override async Task AfterCreateAsync(Role entity, CreateRoleRequest request)
    {
        if (request.PermissionIds.Count > 0)
        {
            await _roleRepository.SetPermissionsAsync(entity.Id, request.PermissionIds);
        }
    }

    protected override async Task AfterUpdateAsync(Role entity, UpdateRoleRequest request)
    {
        await _roleRepository.SetPermissionsAsync(entity.Id, request.PermissionIds);
    }
}