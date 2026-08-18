namespace LUPA.Api.Entities;

public class Module : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Menu> Menus { get; set; } = [];

    public ICollection<Permission> Permissions { get; set; } = [];
}