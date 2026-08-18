using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Base;

namespace LUPA.Api.Services.Interfaces;

public interface IReportService : IBaseService<ReportResponse, CreateReportRequest, UpdateReportRequest>
{
}