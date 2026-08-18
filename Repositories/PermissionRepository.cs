using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;
using LUPA.Api.Repositories.Contracts;

namespace LUPA.Api.Repositories;

public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(ApplicationDbContext context) : base(context)
    {
    }

    protected override IQueryable<Permission> ApplySearch(IQueryable<Permission> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        return query.Where(x =>
            x.Code.Contains(search) ||
            x.Name.Contains(search) ||
            (x.Description != null && x.Description.Contains(search)));
    }

    protected override IQueryable<Permission> ApplySort(IQueryable<Permission> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLower() switch
        {
            "code" => descending ? query.OrderByDescending(x => x.Code) : query.OrderBy(x => x.Code),
            "name" => descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            _ => query.OrderBy(x => x.Id)
        };
    }
}