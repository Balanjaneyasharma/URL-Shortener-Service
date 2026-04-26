using Microsoft.EntityFrameworkCore; // Assuming your ShortUrl model is in Domain
using MyApi.Infrastructure.Entities; // Adjust the namespace if your ShortUrl model is in a different namespace   

namespace MyApi.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // This represents your table in Neon
        public DbSet<UrlMappingEntity> ShortUrlTable => Set<UrlMappingEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Performance Tip: Ensure ShortCode is indexed for fast lookups
            modelBuilder
                .Entity<UrlMappingEntity>()
                .HasIndex(u => u.ShortUrl)
                .IsUnique();
        }
    }
}