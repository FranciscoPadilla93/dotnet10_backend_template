using LUPA.Api.Common;
using LUPA.Api.Common.Charts;
using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Responses;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace LUPA.Api.Services.Audit;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditLogService(IAuditLogRepository auditLogRepository, IHttpContextAccessor httpContextAccessor)
    {
        _auditLogRepository = auditLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(
        string action,
        string entityName,
        string? entityId,
        string? beforeJson,
        string? afterJson,
        int? actorUserId = null,
        string? actorUsername = null)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var claimsUser = httpContext?.User;

        int? userId = actorUserId;

        if (userId is null)
        {
            var subClaim = claimsUser?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (subClaim is not null && int.TryParse(subClaim, out var parsedUserId))
            {
                userId = parsedUserId;
            }
        }

        string? username = actorUsername
            ?? claimsUser?.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;

        string? ipAddress = httpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault()
            ?? httpContext?.Connection.RemoteIpAddress?.ToString();

        var log = new AuditLog
        {
            UserId = userId,
            Username = username,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            BeforeJson = beforeJson,
            AfterJson = afterJson,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        await _auditLogRepository.AddAsync(log);
    }

    public async Task<PagedResponse<AuditLogResponse>> GetPagedAsync(PaginationRequest request)
    {
        var result = await _auditLogRepository.GetPagedAsync(request);

        return new PagedResponse<AuditLogResponse>
        {
            Items = result.Items.Select(ToResponse).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<AuditLogResponse> GetByIdAsync(int id)
    {
        var log = await _auditLogRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Registro de auditoría no encontrado.");

        return ToResponse(log);
    }

    private static AuditLogResponse ToResponse(AuditLog log)
    {
        return new AuditLogResponse
        {
            Id = log.Id,
            UserId = log.UserId,
            Username = log.Username,
            Action = log.Action,
            EntityName = log.EntityName,
            EntityId = log.EntityId,
            BeforeJson = log.BeforeJson,
            AfterJson = log.AfterJson,
            IpAddress = log.IpAddress,
            CreatedAt = log.CreatedAt
        };
    }

    public async Task<byte[]> GenerateActionChartAsync()
    {
        var counts = await _auditLogRepository.GetCountByActionAsync();

        var labels = counts.Keys.ToList();
        var values = counts.Values.Select(x => (double)x).ToList();

        return ChartGenerator.GenerateBarChart("Eventos de auditoría por tipo", labels, values);
    }
}