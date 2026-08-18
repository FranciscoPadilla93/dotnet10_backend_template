using System.ComponentModel.DataAnnotations;

namespace LUPA.Api.Requests.Auth;

public sealed class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}