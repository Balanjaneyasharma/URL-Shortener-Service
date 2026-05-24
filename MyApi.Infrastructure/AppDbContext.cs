using Microsoft.EntityFrameworkCore;
using MyApi.Infrastructure.Entities;

namespace MyApi.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // This represents table in Neon
        public DbSet<UrlMappingEntity> ShortUrlTable => Set<UrlMappingEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Performance: Ensuring ShortCode is indexed for fast lookups
            modelBuilder
                .Entity<UrlMappingEntity>()
                .HasIndex(u => u.ShortUrl)
                .IsUnique();
        }
    }
}