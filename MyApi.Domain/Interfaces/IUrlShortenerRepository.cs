using MyApi.Domain.Models;

namespace MyApi.Domain.Interfaces;

public interface IUrlShortenerRepository
{
    public Task<string> GetLongUrlByShortUrl(string shortUrl);

    public Task<UrlMapping[]> GetAllShortUrls(GetUserShortUrlsRequest request);

    public Task<string> CreateShortUrl(CreateUrlMappingRequest request);

    public Task<bool> DeleteShortUrl(DeleteShortUrlRequest request);

    public Task<string?> GetShortUrlByHashedLongUrl(GetShortUrlByHashedLongUrlRequest request);
}