using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests;

public class CreateRoleRequest
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public List<int> PermissionIds { get; set; } = [];
}