using System.ComponentModel.DataAnnotations;
using LUPA.Api.Entities;

namespace LUPA.Api.Requests;

public class ReportParameterRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Label { get; set; } = string.Empty;

    public ReportParameterType DataType { get; set; }

    public bool IsRequired { get; set; } = true;

    public string? DefaultValue { get; set; }

    public int SortOrder { get; set; }
}