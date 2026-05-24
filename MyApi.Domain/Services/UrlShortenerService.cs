using MyApi.Domain.Interfaces;
using MyApi.Domain.Models;
using MyApi.Domain.Exceptions;

namespace MyApi.Domain.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly IUrlShortenerRepository _urlShortenerRepository;
    private readonly IShortCodeGenerator _shortCodeGenerator;
    private readonly IHashCodeGenerator _hashCodeGenerator;

    public UrlShortenerService(IUrlShortenerRepository repository, IShortCodeGenerator shortCodeGenerator, IHashCodeGenerator hashCodeGenerator)
    {
        _urlShortenerRepository = repository;
        _shortCodeGenerator = shortCodeGenerator;
        _hashCodeGenerator = hashCodeGenerator;
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
        var hashedLongUrl = _hashCodeGenerator.GenerateHash(longUrl);
        var existingShortUrl = await _urlShortenerRepository.GetShortUrlByHashedLongUrl(hashedLongUrl);

        if (existingShortUrl is not null)
        {
            throw new UrlAlreadyShortenedException(longUrl, existingShortUrl);
        }

        const int maxRetries = 3;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            var shortCode = _shortCodeGenerator.Generate();

            try
            {
                return await _urlShortenerRepository.CreateShortUrl(longUrl, shortCode, hashedLongUrl);
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
}
