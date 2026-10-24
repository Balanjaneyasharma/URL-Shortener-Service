using Microsoft.EntityFrameworkCore;
using MyApi.Domain.Interfaces;
using MyApi.Infrastructure.Entities;
using MyApi.Infrastructure.Persistence;

namespace MyApi.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(int userId, string hashedToken, DateTime expiresAt)
    {
        var entity = new RefreshTokenEntity
        {
            UserId = userId,
            HashedToken = hashedToken,
            ExpiresAt = expiresAt,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<int?> GetUserIdByTokenAsync(string hashedToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.HashedToken == hashedToken
                && !r.IsRevoked
                && r.ExpiresAt > DateTime.UtcNow);

        return token?.UserId;
    }

    public async Task<bool> TryConsumeAsync(string hashedToken)
    {
        var affected = await _context.RefreshTokens
            .Where(r => r.HashedToken == hashedToken
                && !r.IsRevoked
                && r.ExpiresAt > DateTime.UtcNow)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsRevoked, true));

        return affected > 0;
    }

    public async Task RevokeAsync(string hashedToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.HashedToken == hashedToken);

        if (token is not null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RevokeAllForUserAsync(int userId)
    {
        await _context.RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsRevoked, true));
    }
}