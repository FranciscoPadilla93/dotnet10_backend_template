using LUPA.Api.Entities;

namespace LUPA.Api.Responses;

public class ReportParameterResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public ReportParameterType DataType { get; set; }

    public bool IsRequired { get; set; }

    public string? DefaultValue { get; set; }

    public int SortOrder { get; set; }
}