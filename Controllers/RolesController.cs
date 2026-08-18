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
[Route("api/roles")]
public class RolesController : BaseController<RoleResponse, CreateRoleRequest, UpdateRoleRequest>
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    protected override IBaseService<RoleResponse, CreateRoleRequest, UpdateRoleRequest> Service
        => _roleService;

    [HasPermission("ROLE_VIEW")]
    [HttpGet]
    public Task<IActionResult> GetRoles([FromQuery] PaginationRequest request)
        => GetPagedInternal(request);

    [HasPermission("ROLE_VIEW")]
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id)
        => GetByIdInternal(id);

    [HasPermission("ROLE_CREATE")]
    [HttpPost]
    public Task<IActionResult> Create(CreateRoleRequest request)
        => CreateInternal(request);

    [HasPermission("ROLE_UPDATE")]
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, UpdateRoleRequest request)
        => UpdateInternal(id, request);

    [HasPermission("ROLE_DELETE")]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id)
        => DeleteInternal(id);
}