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
[Route("api/modules")]
public class ModulesController : BaseController<ModuleResponse, CreateModuleRequest, UpdateModuleRequest>
{
    private readonly IModuleService _moduleService;

    public ModulesController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    protected override IBaseService<ModuleResponse, CreateModuleRequest, UpdateModuleRequest> Service
        => _moduleService;

    [HasPermission("MODULE_VIEW")]
    [HttpGet]
    public Task<IActionResult> GetModules([FromQuery] PaginationRequest request)
        => GetPagedInternal(request);

    [HasPermission("MODULE_VIEW")]
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id)
        => GetByIdInternal(id);

    [HasPermission("MODULE_CREATE")]
    [HttpPost]
    public Task<IActionResult> Create(CreateModuleRequest request)
        => CreateInternal(request);

    [HasPermission("MODULE_UPDATE")]
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, UpdateModuleRequest request)
        => UpdateInternal(id, request);

    [HasPermission("MODULE_DELETE")]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id)
        => DeleteInternal(id);
}