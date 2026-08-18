using LUPA.Api.Common;
using LUPA.Api.Common.Authorization;
using LUPA.Api.Common.Excel;
using LUPA.Api.Common.Pdf;
using LUPA.Api.Controllers.Base;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;
using LUPA.Api.Services.Reports;
using Microsoft.AspNetCore.Mvc;

namespace LUPA.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : BaseController<ReportResponse, CreateReportRequest, UpdateReportRequest>
{
    private readonly IReportService _reportService;
    private readonly IReportExecutionService _executionService;

    public ReportsController(IReportService reportService, IReportExecutionService executionService)
    {
        _reportService = reportService;
        _executionService = executionService;
    }

    protected override IBaseService<ReportResponse, CreateReportRequest, UpdateReportRequest> Service
        => _reportService;

    [HasPermission("REPORT_VIEW")]
    [HttpGet]
    public Task<IActionResult> GetReports([FromQuery] PaginationRequest request)
        => GetPagedInternal(request);

    [HasPermission("REPORT_VIEW")]
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id)
        => GetByIdInternal(id);

    [HasPermission("REPORT_CREATE")]
    [HttpPost]
    public Task<IActionResult> Create(CreateReportRequest request)
        => CreateInternal(request);

    [HasPermission("REPORT_UPDATE")]
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, UpdateReportRequest request)
        => UpdateInternal(id, request);

    [HasPermission("REPORT_DELETE")]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id)
        => DeleteInternal(id);

    [HasPermission("REPORT_EXECUTE")]
    [HttpPost("{id:int}/execute")]
    public async Task<IActionResult> Execute(int id, [FromBody] Dictionary<string, string> parameters)
    {
        var result = await _executionService.ExecuteAsync(id, parameters);

        return Ok(new ApiResponse<List<Dictionary<string, object?>>>
        {
            Success = true,
            Data = result
        });
    }

    [HasPermission("REPORT_EXECUTE")]
    [HttpPost("{id:int}/execute/excel")]
    public async Task<IActionResult> ExecuteExcel(int id, [FromBody] Dictionary<string, string> parameters)
    {
        var result = await _executionService.ExecuteAsync(id, parameters);
        var bytes = ExcelExporter.ExportDynamic(result, "Reporte");

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Reporte.xlsx");
    }

    [HasPermission("REPORT_EXECUTE")]
    [HttpPost("{id:int}/execute/pdf")]
    public async Task<IActionResult> ExecutePdf(int id, [FromBody] Dictionary<string, string> parameters)
    {
        var result = await _executionService.ExecuteAsync(id, parameters);
        var bytes = PdfReportGenerator.GenerateDynamicTableReport(result, "Reporte");

        return File(bytes, "application/pdf", "Reporte.pdf");
    }
}