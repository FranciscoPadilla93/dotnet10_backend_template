namespace LUPA.Api.Responses;

public class RoleResponse
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public IReadOnlyCollection<string> Permissions { get; set; } = [];
}