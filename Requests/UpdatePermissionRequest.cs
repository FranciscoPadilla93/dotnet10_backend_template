using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests;

public class UpdatePermissionRequest
{
    [Required]
    public int ModuleId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}