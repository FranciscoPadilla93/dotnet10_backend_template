using LUPA.Api.Common;
using LUPA.Api.Common.Authorization;
using LUPA.Api.Services.Audit;
using Microsoft.AspNetCore.Mvc;

namespace LUPA.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HasPermission("AUDIT_LOG_VIEW")]
    [HttpGet]
    public async Task<IActionResult> GetAuditLogs([FromQuery] PaginationRequest request)
    {
        var result = await _auditLogService.GetPagedAsync(request);
        return Ok(result);
    }

    [HasPermission("AUDIT_LOG_VIEW")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _auditLogService.GetByIdAsync(id);
        return Ok(result);
    }

    [HasPermission("AUDIT_LOG_VIEW")]
    [HttpGet("chart/by-action")]
    public async Task<IActionResult> ChartByAction()
    {
        var bytes = await _auditLogService.GenerateActionChartAsync();

        return File(bytes, "image/png");
    }
}