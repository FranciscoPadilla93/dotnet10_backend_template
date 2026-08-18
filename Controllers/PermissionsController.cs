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
[Route("api/permissions")]
public class PermissionsController
    : BaseController<PermissionResponse, CreatePermissionRequest, UpdatePermissionRequest>
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override IBaseService<PermissionResponse, CreatePermissionRequest, UpdatePermissionRequest> Service
        => _permissionService;

    [HasPermission("PERMISSION_VIEW")]
    [HttpGet]
    public Task<IActionResult> GetPermissions([FromQuery] PaginationRequest request)
        => GetPagedInternal(request);

    [HasPermission("PERMISSION_VIEW")]
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id)
        => GetByIdInternal(id);

    [HasPermission("PERMISSION_CREATE")]
    [HttpPost]
    public Task<IActionResult> Create(CreatePermissionRequest request)
        => CreateInternal(request);

    [HasPermission("PERMISSION_UPDATE")]
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, UpdatePermissionRequest request)
        => UpdateInternal(id, request);

    [HasPermission("PERMISSION_DELETE")]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id)
        => DeleteInternal(id);
}