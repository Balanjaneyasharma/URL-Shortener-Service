using MyApi.Domain.Models;

namespace MyApi.Domain.Interfaces;

public interface IUrlShortenerRepository
{
    public Task<string> GetLongUrlByShortUrl(string shortUrl);

    public Task<UrlMapping[]> GetAllShortUrls();

    public Task<string> CreateShortUrl(string longUrl, string shortUrl, string hashedLongUrl);

    public Task<bool> DeleteShortUrl(string shortUrl);

     public Task<string?> GetShortUrlByHashedLongUrl(string hashedLongUrl);
}