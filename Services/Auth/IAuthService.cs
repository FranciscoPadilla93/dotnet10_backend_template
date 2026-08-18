using LUPA.Api.Requests.Auth;
using LUPA.Api.Responses.Auth;

namespace LUPA.Api.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress = null);

    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request, string? ipAddress = null);

    Task LogoutAsync(RefreshTokenRequest request, string? ipAddress = null);
}