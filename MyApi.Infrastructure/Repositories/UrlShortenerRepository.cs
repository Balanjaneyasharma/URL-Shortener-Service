using Microsoft.EntityFrameworkCore;
using Npgsql;

using MyApi.Domain.Models;
using MyApi.Domain.Interfaces;
using MyApi.Domain.Exceptions;

using MyApi.Infrastructure.Persistence;
using MyApi.Infrastructure.Entities;
using MyApi.Infrastructure.Mappers;
using MyApi.Infrastructure.Constraints;
using MyApi.Infrastructure.Exceptions;

namespace MyApi.Infrastructure.Repositories;

public class UrlShortenerRepository : IUrlShortenerRepository
{

    private readonly IAppDbContext _dbContext;

    public UrlShortenerRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetLongUrlByShortUrl(string shortUrl)
    {
        var entity = await _dbContext.ShortUrlTable.FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);

        if (entity is null)
        {
            throw new UrlNotFoundException(shortUrl);
        }
        return entity.LongUrl;

    }

    public async Task<UrlMapping[]> GetAllShortUrls(GetUserShortUrlsRequest request)
    {
        var urlMappingEntities = await _dbContext.ShortUrlTable
            .Where(u => u.UserId == request.UserId)
            .ToListAsync();
        var urlMappings = urlMappingEntities.Select(e => e.ToDomain()).ToList();

        return urlMappings.ToArray();
    }

    public async Task<string> CreateShortUrl(CreateUrlMappingRequest request)
    {
        try
        {
            this._dbContext.ShortUrlTable.Add(
                new UrlMappingEntity
                {
                    LongUrl = request.LongUrl,
                    ShortUrl = request.ShortUrl,
                    HashedLongUrl = request.HashedLongUrl,
                    UserId = request.UserId
                }
            );
            await this._dbContext.SaveChangesAsync();
            return request.ShortUrl;
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx)
            {
                // 23505 = unique violation
                if (pgEx.SqlState == "23505")
                {
                    if (pgEx.ConstraintName == ShortUrlDbConstraints.DuplicateShortCode)
                    {
                        throw new DuplicateShortCodeException(request.ShortUrl);
                    }
                }
            }
            // fallback
            throw new DatabaseConnectionException();
        }
    }

    public async Task<bool> DeleteShortUrl(DeleteShortUrlRequest request)
    {
        var deleted = await this._dbContext.ShortUrlTable
            .Where(u => u.ShortUrl == request.ShortUrl && u.UserId == request.UserId)
            .ExecuteDeleteAsync();

        if (deleted == 0)
        {
            throw new UrlNotFoundException(request.ShortUrl);
        }

        return true;
    }

    public async Task<string?> GetShortUrlByHashedLongUrl(GetShortUrlByHashedLongUrlRequest request)
    {
        var shortUrl = await this._dbContext.ShortUrlTable
                        .Where(u => u.HashedLongUrl == request.HashedLongUrl && u.UserId == request.UserId)
                        .Select(u => u.ShortUrl)
                        .FirstOrDefaultAsync();
        if (shortUrl is null)
        {
            return null;
        }
        return shortUrl;
    }
    
}