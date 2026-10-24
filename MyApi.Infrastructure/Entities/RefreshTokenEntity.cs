using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Infrastructure.Entities;

[Table("refresh_tokens")]
public class RefreshTokenEntity
{
    public int Id { get; set; }
    public required string HashedToken { get; set; }
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public UserEntity User { get; set; } = null!;
}