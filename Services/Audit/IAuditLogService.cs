using LUPA.Api.Common;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Audit;

public interface IAuditLogService
{
    Task LogAsync(
        string action,
        string entityName,
        string? entityId,
        string? beforeJson,
        string? afterJson,
        int? actorUserId = null,
        string? actorUsername = null);

    Task<PagedResponse<AuditLogResponse>> GetPagedAsync(PaginationRequest request);
    Task<AuditLogResponse> GetByIdAsync(int id);
    Task<byte[]> GenerateActionChartAsync();
}