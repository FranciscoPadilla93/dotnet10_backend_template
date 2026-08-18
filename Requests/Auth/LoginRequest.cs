using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests.Auth;

public sealed class LoginRequest
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
