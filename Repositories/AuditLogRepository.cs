using LUPA.Api.Common;
using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly ApplicationDbContext _context;

    public AuditLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog log)
    {
        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task<AuditLog?> GetByIdAsync(int id)
    {
        return await _context.AuditLogs.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedResult<AuditLog>> GetPagedAsync(PaginationRequest request)
    {
        IQueryable<AuditLog> query = _context.AuditLogs;

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.EntityName.Contains(request.Search) ||
                x.Action.Contains(request.Search) ||
                (x.Username != null && x.Username.Contains(request.Search)));
        }

        var totalRecords = await query.CountAsync();

        query = query.OrderByDescending(x => x.CreatedAt);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<AuditLog>
        {
            Items = items,
            TotalRecords = totalRecords
        };
    }

    public async Task<Dictionary<string, int>> GetCountByActionAsync()
    {
        return await _context.AuditLogs
            .GroupBy(x => x.Action)
            .Select(g => new { Action = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Action, x => x.Count);
    }
}