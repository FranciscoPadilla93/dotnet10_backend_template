using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
    }

    public async Task RevokeAllActiveForUserAsync(int userId, string? revokedByIp = null)
    {
        var activeTokens = await _context.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null)
            .ToListAsync();

        var now = DateTime.UtcNow;

        foreach (var token in activeTokens)
        {
            if (token.IsExpired)
            {
                continue;
            }

            token.RevokedAt = now;
            token.RevokedByIp = revokedByIp;
        }

        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}