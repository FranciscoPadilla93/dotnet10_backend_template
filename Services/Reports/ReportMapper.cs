using LUPA.Api.Entities;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Reports;

public static class ReportMapper
{
    public static ReportResponse ToResponse(Report report)
    {
        return new ReportResponse
        {
            Id = report.Id,
            Code = report.Code,
            Name = report.Name,
            Description = report.Description,
            StoredProcedureName = report.StoredProcedureName,
            IsActive = report.IsActive,
            Parameters = report.Parameters
                .OrderBy(p => p.SortOrder)
                .Select(p => new ReportParameterResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Label = p.Label,
                    DataType = p.DataType,
                    IsRequired = p.IsRequired,
                    DefaultValue = p.DefaultValue,
                    SortOrder = p.SortOrder
                })
                .ToList()
        };
    }
}