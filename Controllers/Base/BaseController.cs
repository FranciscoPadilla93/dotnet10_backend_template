using LUPA.Api.Common;
using LUPA.Api.Common.Excel;
using LUPA.Api.Common.Pdf;
using LUPA.Api.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace LUPA.Api.Controllers.Base;

/// <summary>
/// Wiring genérico de CRUD sobre un IBaseService. El controller concreto (ej. UsersController)
/// hereda de esta clase, expone la ruta/[Http*] real de cada acción, le pone el [HasPermission]
/// que corresponda a SU módulo, y delega en los métodos *Internal de aquí.
/// </summary>
public abstract class BaseController<TResponse, TCreateRequest, TUpdateRequest> : ControllerBase
{
    protected abstract IBaseService<TResponse, TCreateRequest, TUpdateRequest> Service { get; }

    protected async Task<IActionResult> GetPagedInternal([FromQuery] PaginationRequest request)
    {
        var result = await Service.GetPagedAsync(request);

        return Ok(result);
    }

    protected async Task<IActionResult> GetByIdInternal(int id)
    {
        var result = await Service.GetByIdAsync(id);

        return Ok(result);
    }

    protected async Task<IActionResult> CreateInternal(TCreateRequest request)
    {
        var result = await Service.CreateAsync(request);

        return Ok(new ApiResponse<TResponse>
        {
            Success = true,
            Message = "Registro creado correctamente.",
            Data = result
        });
    }

    protected async Task<IActionResult> UpdateInternal(int id, TUpdateRequest request)
    {
        var result = await Service.UpdateAsync(id, request);

        return Ok(new ApiResponse<TResponse>
        {
            Success = true,
            Message = "Registro actualizado correctamente.",
            Data = result
        });
    }

    protected async Task<IActionResult> DeleteInternal(int id)
    {
        await Service.DeleteAsync(id);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Registro eliminado correctamente."
        });
    }

    /// <summary>
    /// Exporta TODO el listado (ignora paginación) a un .xlsx descargable.
    /// Usa un PageSize grande porque IBaseService no expone un "GetAll" dedicado;
    /// para catálogos que puedan crecer mucho, considera agregar ese método más adelante.
    /// </summary>
    protected async Task<IActionResult> ExportInternal(string fileName)
    {
        var result = await Service.GetPagedAsync(new PaginationRequest
        {
            Page = 1,
            PageSize = 1_000_000
        });

        var bytes = ExcelExporter.Export(result.Items, fileName);

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{fileName}.xlsx");
    }

    protected async Task<IActionResult> ExportPdfInternal(string title, string fileName)
    {
        var result = await Service.GetPagedAsync(new PaginationRequest
        {
            Page = 1,
            PageSize = 1_000_000
        });

        var bytes = PdfReportGenerator.GenerateTableReport(result.Items, title);

        return File(bytes, "application/pdf", $"{fileName}.pdf");
    }
}