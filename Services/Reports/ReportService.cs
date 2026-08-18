using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Audit;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;

namespace LUPA.Api.Services.Reports;

public class ReportService
    : BaseService<Report, ReportResponse, CreateReportRequest, UpdateReportRequest>, IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository, IAuditLogService auditLogService)
        : base(reportRepository, auditLogService)
    {
        _reportRepository = reportRepository;
    }

    protected override string NotFoundMessage => "Reporte no encontrado.";

    protected override Task<ReportResponse> MapToResponseAsync(Report entity)
    {
        return Task.FromResult(ReportMapper.ToResponse(entity));
    }

    protected override async Task<Report> MapToEntityAsync(CreateReportRequest request)
    {
        bool codeInUse = await _reportRepository.ExistsAsync(x => x.Code == request.Code);

        if (codeInUse)
        {
            throw new ConflictException($"Ya existe un reporte con el código '{request.Code}'.");
        }

        return new Report
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            StoredProcedureName = request.StoredProcedureName,
            IsActive = true
        };
    }

    protected override Task ApplyUpdateAsync(Report entity, UpdateReportRequest request)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.StoredProcedureName = request.StoredProcedureName;
        entity.IsActive = request.IsActive;

        return Task.CompletedTask;
    }

    protected override async Task AfterCreateAsync(Report entity, CreateReportRequest request)
    {
        await _reportRepository.SetParametersAsync(entity.Id, MapParameters(request.Parameters));
    }

    protected override async Task AfterUpdateAsync(Report entity, UpdateReportRequest request)
    {
        await _reportRepository.SetParametersAsync(entity.Id, MapParameters(request.Parameters));
    }

    private static List<ReportParameter> MapParameters(List<ReportParameterRequest> parameters)
    {
        return parameters.Select(p => new ReportParameter
        {
            Name = p.Name,
            Label = p.Label,
            DataType = p.DataType,
            IsRequired = p.IsRequired,
            DefaultValue = p.DefaultValue,
            SortOrder = p.SortOrder
        }).ToList();
    }
}