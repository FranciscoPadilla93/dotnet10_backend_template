using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests;

public class CreateReportRequest
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(150)]
    public string StoredProcedureName { get; set; } = string.Empty;

    public List<ReportParameterRequest> Parameters { get; set; } = [];
}