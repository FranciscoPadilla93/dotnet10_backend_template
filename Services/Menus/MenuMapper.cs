using LUPA.Api.Entities;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Menus;

public static class MenuMapper
{
    public static MenuResponse ToResponse(Menu menu)
    {
        return new MenuResponse
        {
            Id = menu.Id,
            ModuleId = menu.ModuleId,
            ParentMenuId = menu.ParentMenuId,
            Code = menu.Code,
            Name = menu.Name,
            Route = menu.Route,
            Icon = menu.Icon,
            SortOrder = menu.SortOrder,
            IsVisible = menu.IsVisible,
            IsActive = menu.IsActive
        };
    }
}