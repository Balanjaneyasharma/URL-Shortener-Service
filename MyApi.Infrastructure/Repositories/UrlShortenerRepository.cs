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

    public async Task<UrlMapping[]> GetAllShortUrls()
    {
        var urlMappingEntities = await _dbContext.ShortUrlTable.ToListAsync();
        var urlMappings = urlMappingEntities.Select(e => e.ToDomain()).ToList();

        return urlMappings.ToArray();
    }

    public async Task<string> CreateShortUrl(string longUrl, string shortUrl, string hashedLongUrl)
    {
        try
        {
            this._dbContext.ShortUrlTable.Add(
                new UrlMappingEntity 
                { 
                    LongUrl = longUrl, 
                    ShortUrl = shortUrl, 
                    HashedLongUrl = hashedLongUrl
                }
            );
            await this._dbContext.SaveChangesAsync();
            return shortUrl;  
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
                        throw new DuplicateShortCodeException(shortUrl);
                    }
                }
            }
            // fallback
            throw new DatabaseConnectionException();
        }
    }

    public async Task<bool> DeleteShortUrl(string shortUrl)
    {
        await this._dbContext.ShortUrlTable.Where(u => u.ShortUrl == shortUrl).ExecuteDeleteAsync();
        return true;
    }

    public async Task<string?> GetShortUrlByHashedLongUrl(string hashedLongUrl)
    {
        var shortUrl = await this._dbContext.ShortUrlTable
                        .Where(u => u.HashedLongUrl == hashedLongUrl)
                        .Select(u => u.ShortUrl)
                        .FirstOrDefaultAsync();
        if (shortUrl is null)
        {
            return null;
        }
        return shortUrl;
    }
    
}