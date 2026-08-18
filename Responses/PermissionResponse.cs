namespace LUPA.Api.Responses;

public class PermissionResponse
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}