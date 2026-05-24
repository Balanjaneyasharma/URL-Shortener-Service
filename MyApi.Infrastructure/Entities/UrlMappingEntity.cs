namespace MyApi.Infrastructure.Entities;
public class UrlMappingEntity
{
    public int Id { get; set; }
    public string ShortUrl { get; set; } = null!;
    public string LongUrl { get; set; } = null!;
    public string? HashedLongUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}