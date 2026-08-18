using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;

namespace LUPA.Api.Repositories.Contracts;

public interface IReportRepository : IBaseRepository<Report>
{
    Task SetParametersAsync(int reportId, IEnumerable<ReportParameter> parameters);
}