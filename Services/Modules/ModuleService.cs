using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Audit;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;

namespace LUPA.Api.Services.Modules;

public class ModuleService
    : BaseService<Module, ModuleResponse, CreateModuleRequest, UpdateModuleRequest>, IModuleService
{
    private readonly IModuleRepository _moduleRepository;

    public ModuleService(IModuleRepository moduleRepository, IAuditLogService auditLogService)
        : base(moduleRepository, auditLogService)
    {
        _moduleRepository = moduleRepository;
    }

    protected override string NotFoundMessage => "Módulo no encontrado.";

    protected override Task<ModuleResponse> MapToResponseAsync(Module entity)
    {
        return Task.FromResult(ModuleMapper.ToResponse(entity));
    }

    protected override async Task<Module> MapToEntityAsync(CreateModuleRequest request)
    {
        bool codeInUse = await _moduleRepository.ExistsAsync(x => x.Code == request.Code);

        if (codeInUse)
        {
            throw new ConflictException($"Ya existe un módulo con el código '{request.Code}'.");
        }

        return new Module
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Icon = request.Icon,
            SortOrder = request.SortOrder,
            IsActive = true
        };
    }

    protected override Task ApplyUpdateAsync(Module entity, UpdateModuleRequest request)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Icon = request.Icon;
        entity.SortOrder = request.SortOrder;
        entity.IsActive = request.IsActive;

        return Task.CompletedTask;
    }
}