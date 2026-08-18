namespace LUPA.Api.Responses;

public class ReportResponse
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string StoredProcedureName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public List<ReportParameterResponse> Parameters { get; set; } = [];
}