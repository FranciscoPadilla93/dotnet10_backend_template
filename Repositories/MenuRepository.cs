using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;
using LUPA.Api.Repositories.Contracts;

namespace LUPA.Api.Repositories;

public class MenuRepository : BaseRepository<Menu>, IMenuRepository
{
    public MenuRepository(ApplicationDbContext context) : base(context)
    {
    }

    protected override IQueryable<Menu> ApplySearch(IQueryable<Menu> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        return query.Where(x =>
            x.Code.Contains(search) ||
            x.Name.Contains(search) ||
            x.Route.Contains(search));
    }

    protected override IQueryable<Menu> ApplySort(IQueryable<Menu> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLower() switch
        {
            "code" => descending ? query.OrderByDescending(x => x.Code) : query.OrderBy(x => x.Code),
            "name" => descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            _ => descending ? query.OrderByDescending(x => x.SortOrder) : query.OrderBy(x => x.SortOrder)
        };
    }
}