using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests;

public class UpdateReportRequest
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(150)]
    public string StoredProcedureName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public List<ReportParameterRequest> Parameters { get; set; } = [];
}