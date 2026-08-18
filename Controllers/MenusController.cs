using LUPA.Api.Common;
using LUPA.Api.Common.Authorization;
using LUPA.Api.Controllers.Base;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LUPA.Api.Controllers;

[ApiController]
[Route("api/menus")]
public class MenusController : BaseController<MenuResponse, CreateMenuRequest, UpdateMenuRequest>
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    protected override IBaseService<MenuResponse, CreateMenuRequest, UpdateMenuRequest> Service
        => _menuService;

    [HasPermission("MENU_VIEW")]
    [HttpGet]
    public Task<IActionResult> GetMenus([FromQuery] PaginationRequest request)
        => GetPagedInternal(request);

    [HasPermission("MENU_VIEW")]
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id)
        => GetByIdInternal(id);

    [HasPermission("MENU_CREATE")]
    [HttpPost]
    public Task<IActionResult> Create(CreateMenuRequest request)
        => CreateInternal(request);

    [HasPermission("MENU_UPDATE")]
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, UpdateMenuRequest request)
        => UpdateInternal(id, request);

    [HasPermission("MENU_DELETE")]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id)
        => DeleteInternal(id);
}