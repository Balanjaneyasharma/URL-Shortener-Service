using MyApi.Domain.Interfaces;
using MyApi.Domain.Models;
using MyApi.Domain.Exceptions;

namespace MyApi.Domain.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly IUrlShortenerRepository _urlShortenerRepository;
    private readonly IShortCodeGenerator _shortCodeGenerator;
    private readonly IHashCodeGenerator _hashCodeGenerator;
    private readonly ICurrentUserService _currentUserService;

    public UrlShortenerService(
        IUrlShortenerRepository repository,
        IShortCodeGenerator shortCodeGenerator,
        IHashCodeGenerator hashCodeGenerator,
        ICurrentUserService currentUserService)
    {
        _urlShortenerRepository = repository;
        _shortCodeGenerator = shortCodeGenerator;
        _hashCodeGenerator = hashCodeGenerator;
        _currentUserService = currentUserService;
    }

    public async Task<string> GetLongUrlByShortUrl(string shortUrl)
    {
        return await _urlShortenerRepository.GetLongUrlByShortUrl(shortUrl);
    }

    public async Task<UrlMapping[]> GetAllShortUrls()
    {
        return await _urlShortenerRepository.GetAllShortUrls(
            new GetUserShortUrlsRequest(_currentUserService.UserId)
        );
    }

    public async Task<string> CreateShortUrl(CreateShortUrlRequest request)
    {
        var hashedLongUrl = _hashCodeGenerator.GenerateHash(request.LongUrl);
        var userId = _currentUserService.UserId;
        var existingShortUrl = await _urlShortenerRepository.GetShortUrlByHashedLongUrl(
            new GetShortUrlByHashedLongUrlRequest(hashedLongUrl, userId)
        );

        if (existingShortUrl is not null)
        {
            throw new UrlAlreadyShortenedException(request.LongUrl, existingShortUrl);
        }

        const int maxRetries = 3;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            var shortCode = _shortCodeGenerator.Generate();

            try
            {
                return await _urlShortenerRepository.CreateShortUrl(
                    new CreateUrlMappingRequest(request.LongUrl, shortCode, hashedLongUrl, userId)
                );
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
        return await _urlShortenerRepository.DeleteShortUrl(
            new DeleteShortUrlRequest(shortUrl, _currentUserService.UserId)
        );
    }
}
