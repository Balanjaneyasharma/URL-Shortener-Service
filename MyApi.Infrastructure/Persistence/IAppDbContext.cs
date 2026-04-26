using Microsoft.EntityFrameworkCore;
using MyApi.Infrastructure.Entities;

namespace MyApi.Infrastructure.Persistence;

public interface IAppDbContext
{
    DbSet<UrlMappingEntity> ShortUrlTable { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}