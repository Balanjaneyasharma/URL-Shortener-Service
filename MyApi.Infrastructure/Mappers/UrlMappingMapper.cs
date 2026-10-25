using MyApi.Domain.Models;
using MyApi.Infrastructure.Entities;

namespace MyApi.Infrastructure.Mappers;

public static class UrlMappingMapper
{
    public static UrlMapping ToDomain(this UrlMappingEntity entity) => new()
    {
        Id       = entity.Id,
        LongUrl  = entity.LongUrl,
        ShortUrl = entity.ShortUrl,
        UserId   = entity.UserId   // ← add
    };

    public static UrlMappingEntity ToEntity(this UrlMapping domain) => new()
    {
        Id        = domain.Id,
        LongUrl   = domain.LongUrl,
        ShortUrl  = domain.ShortUrl,
        UserId    = domain.UserId,  // ← add
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
}