namespace LUPA.Api.Entities;

public class Menu : BaseEntity
{
    public int ModuleId { get; set; }

    public int? ParentMenuId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Route { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public bool IsVisible { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public Module Module { get; set; } = null!;

    public Menu? ParentMenu { get; set; }

    public ICollection<Menu> Children { get; set; } = [];
}