using MyApi.Domain.Interfaces;
using MyApi.Domain.Models;
using MyApi.Domain.Exceptions;

namespace MyApi.Domain.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly IUrlShortenerRepository _urlShortenerRepository;
    private readonly IShortCodeGenerator _shortCodeGenerator;

    public UrlShortenerService(IUrlShortenerRepository repository, IShortCodeGenerator shortCodeGenerator)
    {
        _urlShortenerRepository = repository;
        _shortCodeGenerator = shortCodeGenerator;
    }

    public async Task<string> GetLongUrlByShortUrl(string shortUrl)
    {
        return await _urlShortenerRepository.GetLongUrlByShortUrl(shortUrl);
    }

    public async Task<UrlMapping[]> GetAllShortUrls()
    {
        return await _urlShortenerRepository.GetAllShortUrls();
    }

    public async Task<string> CreateShortUrl(string longUrl)
    {
        const int maxRetries = 3;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            var shortCode = _shortCodeGenerator.Generate();

            try
            {
                return await _urlShortenerRepository.CreateShortUrl(longUrl, shortCode);
            }
            catch (DuplicateShortCodeException)
            {
                if (attempt == maxRetries)
                {
                    throw new ShortCodeGenerationFailedException(
                        $"Failed after {maxRetries} attempts"
                    );
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        
        throw new ShortCodeGenerationFailedException("Unexpected failure");
    }

    public async Task<bool> DeleteShortUrl(string shortUrl)
    {
        return await _urlShortenerRepository.DeleteShortUrl(shortUrl);
    }

    private async Task<bool> IsUrlAlreadyShortened(string longUrl)
    {
        return await this._urlShortenerRepository.IsUrlAlreadyShortened(longUrl);
    }
}