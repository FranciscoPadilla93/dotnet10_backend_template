namespace LUPA.Api.Responses;

public class MenuResponse
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public int? ParentMenuId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; }
    public bool IsActive { get; set; }
}