using LUPA.Api.Common;
using LUPA.Api.Common.Authorization;
using LUPA.Api.Common.Excel;
using LUPA.Api.Controllers.Base;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LUPA.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : BaseController<UserResponse, CreateUserRequest, UpdateUserRequest>
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    protected override IBaseService<UserResponse, CreateUserRequest, UpdateUserRequest> Service
        => _userService;

    [HasPermission("USER_VIEW")]
    [HttpGet]
    public Task<IActionResult> GetUsers([FromQuery] PaginationRequest request)
        => GetPagedInternal(request);

    [HasPermission("USER_VIEW")]
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id)
        => GetByIdInternal(id);

    [HasPermission("USER_CREATE")]
    [HttpPost]
    public Task<IActionResult> Create(CreateUserRequest request)
        => CreateInternal(request);

    [HasPermission("USER_UPDATE")]
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, UpdateUserRequest request)
        => UpdateInternal(id, request);

    [HasPermission("USER_DELETE")]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id)
        => DeleteInternal(id);

    [HasPermission("USER_UPDATE")]
    [HttpPatch("{id:int}/activate")]
    public async Task<IActionResult> Activate(int id, [FromQuery] bool isActive = true)
    {
        await _userService.ActivateAsync(id, isActive);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = isActive ? "Usuario activado." : "Usuario desactivado."
        });
    }

    [HasPermission("USER_UPDATE")]
    [HttpPost("{id:int}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, ChangePasswordRequest request)
    {
        await _userService.ChangePasswordAsync(id, request);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Contraseña actualizada correctamente."
        });
    }

    [HasPermission("USER_VIEW")]
    [HttpGet("export")]
    public Task<IActionResult> Export()
        => ExportInternal("Usuarios");

    [HasPermission("USER_CREATE")]
    [HttpGet("import-template")]
    public IActionResult DownloadImportTemplate()
    {
        var bytes = ExcelExporter.Export(
            Enumerable.Empty<CreateUserRequest>(), "Usuarios");

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "PlantillaUsuarios.xlsx");
    }

    [HasPermission("USER_CREATE")]
    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var result = await _userService.ImportFromExcelAsync(stream);

        return Ok(new ApiResponse<ExcelImportResult>
        {
            Success = true,
            Message = $"Importación completada: {result.SuccessCount}/{result.TotalRows} filas.",
            Data = result
        });
    }

    [HasPermission("USER_VIEW")]
    [HttpGet("pdf")]
    public Task<IActionResult> ExportPdf() => ExportPdfInternal("Reporte de Usuarios", "Usuarios");
}