using LUPA.Api.Entities;

namespace LUPA.Api.Repositories.Contracts;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

    /// <summary>
    /// Revoca todos los refresh tokens activos de un usuario.
    /// Se usa como respuesta ante reuso de un token ya revocado (posible robo).
    /// </summary>
    Task RevokeAllActiveForUserAsync(int userId, string? revokedByIp = null);

    Task SaveChangesAsync();
}