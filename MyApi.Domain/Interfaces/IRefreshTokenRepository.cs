namespace MyApi.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task CreateAsync(int userId, string hashedToken, DateTime expiresAt);
    Task<int?> GetUserIdByTokenAsync(string hashedToken);
    Task<bool> TryConsumeAsync(string hashedToken);
    Task RevokeAsync(string hashedToken);
    Task RevokeAllForUserAsync(int userId);
}