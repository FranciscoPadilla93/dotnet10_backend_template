using LUPA.Api.Common;
using LUPA.Api.Requests.Auth;
using LUPA.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LUPA.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(IAuthService authService, IPasswordHasher passwordHasher)
    {
        _authService = authService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request, GetIpAddress());

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request, GetIpAddress());

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequest request)
    {
        await _authService.LogoutAsync(request, GetIpAddress());

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Sesión cerrada correctamente."
        });
    }

    [HttpGet("hash/{password}")]
    public IActionResult Hash(string password)
    {
        return Ok(_passwordHasher.Hash(password));
    }

    private string? GetIpAddress()
    {
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
        {
            return forwarded.ToString();
        }

        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}