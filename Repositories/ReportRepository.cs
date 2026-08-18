using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;
using LUPA.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Repositories;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
    }

    protected override IQueryable<Report> BaseQuery()
    {
        return base.BaseQuery().Include(x => x.Parameters);
    }

    public async Task SetParametersAsync(int reportId, IEnumerable<ReportParameter> parameters)
    {
        var current = await Context.ReportParameters
            .Where(x => x.ReportId == reportId)
            .ToListAsync();

        Context.ReportParameters.RemoveRange(current);

        foreach (var parameter in parameters)
        {
            parameter.ReportId = reportId;
            Context.ReportParameters.Add(parameter);
        }

        await Context.SaveChangesAsync();
    }

    protected override IQueryable<Report> ApplySearch(IQueryable<Report> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        return query.Where(x =>
            x.Code.Contains(search) ||
            x.Name.Contains(search));
    }

    protected override IQueryable<Report> ApplySort(IQueryable<Report> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLower() switch
        {
            "code" => descending ? query.OrderByDescending(x => x.Code) : query.OrderBy(x => x.Code),
            "name" => descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            _ => query.OrderBy(x => x.Id)
        };
    }
}