// Infrastructure/Mappers/ShortUrlMapper.cs
using MyApi.Domain.Models;
using MyApi.Infrastructure.Entities;

namespace MyApi.Infrastructure.Mappers;

public static class UrlMappingMapper
{
    // Entity → Domain model (used when reading FROM db)
    public static UrlMapping ToDomain(this UrlMappingEntity entity) => new()
    {
        Id      = entity.Id,
        LongUrl = entity.LongUrl,
        ShortUrl = entity.ShortUrl
    };

    // Domain model → Entity (used when writing TO db)
    public static UrlMappingEntity ToEntity(this UrlMapping domain) => new()
    {
        Id        = domain.Id,
        LongUrl   = domain.LongUrl,
        ShortUrl  = domain.ShortUrl,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
}