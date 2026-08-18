using LUPA.Api.Common;
using LUPA.Api.Entities;

namespace LUPA.Api.Repositories.Contracts;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);
    Task<AuditLog?> GetByIdAsync(int id);
    Task<PagedResult<AuditLog>> GetPagedAsync(PaginationRequest request);
    Task<Dictionary<string, int>> GetCountByActionAsync();
}