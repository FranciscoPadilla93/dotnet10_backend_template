namespace LUPA.Api.Entities;

public class Permission : BaseEntity
{
    public int ModuleId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Module Module { get; set; } = null!;

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}