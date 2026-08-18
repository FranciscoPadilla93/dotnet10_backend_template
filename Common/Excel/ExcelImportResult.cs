namespace LUPA.Api.Common.Excel;

public class ExcelImportResult
{
    public int TotalRows { get; set; }

    public int SuccessCount { get; set; }

    public List<string> Errors { get; set; } = [];
}