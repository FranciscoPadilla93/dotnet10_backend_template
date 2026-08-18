using LUPA.Api.Common.Exceptions;
using LUPA.Api.Configuration;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Requests.Auth;
using LUPA.Api.Responses.Auth;
using LUPA.Api.Services.Audit;
using Microsoft.Extensions.Options;

namespace LUPA.Api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        IAuditLogService auditLogService,
        IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _auditLogService = auditLogService;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress = null)
    {
        User? user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null || !user.IsActive)
        {
            await _auditLogService.LogAsync(
                "LOGIN_FAILED", "Auth", request.Username,
                beforeJson: null, afterJson: null,
                actorUsername: request.Username);

            throw new UnauthorizedException("Usuario o contraseña incorrectos.");
        }

        bool validPassword = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!validPassword)
        {
            await _auditLogService.LogAsync(
                "LOGIN_FAILED", "Auth", request.Username,
                beforeJson: null, afterJson: null,
                actorUsername: request.Username);

            throw new UnauthorizedException("Usuario o contraseña incorrectos.");
        }

        user.LastLogin = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        var response = await IssueTokensAsync(user, ipAddress);

        await _auditLogService.LogAsync(
            "LOGIN", "Auth", user.Id.ToString(),
            beforeJson: null, afterJson: null,
            actorUserId: user.Id, actorUsername: user.Username);

        return response;
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request, string? ipAddress = null)
    {
        string incomingHash = _jwtService.HashToken(request.RefreshToken);

        RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenHashAsync(incomingHash);

        if (storedToken is null)
        {
            throw new UnauthorizedException("Refresh token inválido.");
        }

        if (!storedToken.IsActive)
        {
            // Reuso de un token ya revocado (pero aún no expirado) es señal de robo:
            // se mata toda la cadena de tokens del usuario como medida de contención.
            if (storedToken.IsRevoked && !storedToken.IsExpired)
            {
                await _refreshTokenRepository.RevokeAllActiveForUserAsync(storedToken.UserId, ipAddress);
            }

            throw new UnauthorizedException("Refresh token inválido o expirado.");
        }

        User? user = await _userRepository.GetByIdAsync(storedToken.UserId);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedException("Usuario no disponible.");
        }

        LoginResponse response = await IssueTokensAsync(user, ipAddress);

        // Rotación: el token usado queda revocado y enlazado al que lo reemplazó.
        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.RevokedByIp = ipAddress;
        storedToken.ReplacedByTokenHash = _jwtService.HashToken(response.RefreshToken);

        await _refreshTokenRepository.SaveChangesAsync();

        return response;
    }

    public async Task LogoutAsync(RefreshTokenRequest request, string? ipAddress = null)
    {
        string incomingHash = _jwtService.HashToken(request.RefreshToken);

        RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenHashAsync(incomingHash);

        if (storedToken is null || !storedToken.IsActive)
        {
            // Logout es idempotente: si el token no existe o ya está inactivo, no es un error.
            return;
        }

        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.RevokedByIp = ipAddress;

        await _refreshTokenRepository.SaveChangesAsync();
    }

    private async Task<LoginResponse> IssueTokensAsync(User user, string? ipAddress)
    {
        List<string> roles = await _userRepository.GetRolesAsync(user.Id);
        List<string> permissions = await _userRepository.GetPermissionCodesAsync(user.Id);

        string accessToken = _jwtService.GenerateAccessToken(user, roles, permissions);
        string refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = _jwtService.HashToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            CreatedByIp = ipAddress
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes)
        };
    }
}