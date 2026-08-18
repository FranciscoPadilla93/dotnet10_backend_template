using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests;

public class UpdateUserRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public List<int> RoleIds { get; set; } = [];
}
