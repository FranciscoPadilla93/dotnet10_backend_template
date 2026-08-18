using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Audit;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;

namespace LUPA.Api.Services.Permissions;

public class PermissionService
    : BaseService<Permission, PermissionResponse, CreatePermissionRequest, UpdatePermissionRequest>,
      IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IModuleRepository _moduleRepository;

    public PermissionService(
        IPermissionRepository permissionRepository,
        IModuleRepository moduleRepository,
        IAuditLogService auditLogService)
        : base(permissionRepository, auditLogService)
    {
        _permissionRepository = permissionRepository;
        _moduleRepository = moduleRepository;
    }

    protected override string NotFoundMessage => "Permiso no encontrado.";

    protected override Task<PermissionResponse> MapToResponseAsync(Permission entity)
    {
        return Task.FromResult(PermissionMapper.ToResponse(entity));
    }

    protected override async Task<Permission> MapToEntityAsync(CreatePermissionRequest request)
    {
        bool codeInUse = await _permissionRepository.ExistsAsync(x => x.Code == request.Code);

        if (codeInUse)
        {
            throw new ConflictException($"Ya existe un permiso con el código '{request.Code}'.");
        }

        await ValidateModuleAsync(request.ModuleId);

        return new Permission
        {
            ModuleId = request.ModuleId,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description
        };
    }

    protected override async Task ApplyUpdateAsync(Permission entity, UpdatePermissionRequest request)
    {
        await ValidateModuleAsync(request.ModuleId);

        entity.ModuleId = request.ModuleId;
        entity.Name = request.Name;
        entity.Description = request.Description;
    }

    private async Task ValidateModuleAsync(int moduleId)
    {
        bool moduleExists = await _moduleRepository.ExistsAsync(x => x.Id == moduleId);

        if (!moduleExists)
        {
            throw new ValidationException($"El módulo con Id {moduleId} no existe.");
        }
    }
}