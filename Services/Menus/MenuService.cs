using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Audit;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;

namespace LUPA.Api.Services.Menus;

public class MenuService
    : BaseService<Menu, MenuResponse, CreateMenuRequest, UpdateMenuRequest>, IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IModuleRepository _moduleRepository;

    public MenuService(
        IMenuRepository menuRepository,
        IModuleRepository moduleRepository,
        IAuditLogService auditLogService)
        : base(menuRepository, auditLogService)
    {
        _menuRepository = menuRepository;
        _moduleRepository = moduleRepository;
    }

    protected override string NotFoundMessage => "Menú no encontrado.";

    protected override Task<MenuResponse> MapToResponseAsync(Menu entity)
    {
        return Task.FromResult(MenuMapper.ToResponse(entity));
    }

    protected override async Task<Menu> MapToEntityAsync(CreateMenuRequest request)
    {
        bool codeInUse = await _menuRepository.ExistsAsync(x => x.Code == request.Code);

        if (codeInUse)
        {
            throw new ConflictException($"Ya existe un menú con el código '{request.Code}'.");
        }

        await ValidateForeignKeysAsync(request.ModuleId, request.ParentMenuId);

        return new Menu
        {
            ModuleId = request.ModuleId,
            ParentMenuId = request.ParentMenuId,
            Code = request.Code,
            Name = request.Name,
            Route = request.Route,
            Icon = request.Icon,
            SortOrder = request.SortOrder,
            IsVisible = request.IsVisible,
            IsActive = true
        };
    }

    protected override async Task ApplyUpdateAsync(Menu entity, UpdateMenuRequest request)
    {
        await ValidateForeignKeysAsync(request.ModuleId, request.ParentMenuId);

        entity.ModuleId = request.ModuleId;
        entity.ParentMenuId = request.ParentMenuId;
        entity.Name = request.Name;
        entity.Route = request.Route;
        entity.Icon = request.Icon;
        entity.SortOrder = request.SortOrder;
        entity.IsVisible = request.IsVisible;
        entity.IsActive = request.IsActive;
    }

    private async Task ValidateForeignKeysAsync(int moduleId, int? parentMenuId)
    {
        bool moduleExists = await _moduleRepository.ExistsAsync(x => x.Id == moduleId);

        if (!moduleExists)
        {
            throw new ValidationException($"El módulo con Id {moduleId} no existe.");
        }

        if (parentMenuId.HasValue)
        {
            bool parentExists = await _menuRepository.ExistsAsync(x => x.Id == parentMenuId.Value);

            if (!parentExists)
            {
                throw new ValidationException($"El menú padre con Id {parentMenuId.Value} no existe.");
            }
        }
    }
}