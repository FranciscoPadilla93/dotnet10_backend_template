using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests;

public class UpdateRoleRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public List<int> PermissionIds { get; set; } = [];
}