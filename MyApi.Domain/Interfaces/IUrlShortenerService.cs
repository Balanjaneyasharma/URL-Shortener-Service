using MyApi.Domain.Models;

namespace MyApi.Domain.Interfaces;

public interface IUrlShortenerService
{
    Task<string> GetLongUrlByShortUrl(string shortUrl);

    Task<UrlMapping[]> GetAllShortUrls();

    Task<string> CreateShortUrl(string longUrl);

    Task<bool> DeleteShortUrl(string shortUrl);
    
}