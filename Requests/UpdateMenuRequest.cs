using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests;

public class UpdateMenuRequest
{
    [Required]
    public int ModuleId { get; set; }

    public int? ParentMenuId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Route { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public bool IsVisible { get; set; } = true;

    public bool IsActive { get; set; } = true;
}