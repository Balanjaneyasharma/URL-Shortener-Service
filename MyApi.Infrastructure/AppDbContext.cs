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

        public DbSet<UserEntity> Users => Set<UserEntity>();
        
        public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Performance: Ensuring ShortCode is indexed for fast lookups
            modelBuilder
                .Entity<UrlMappingEntity>()
                .HasIndex(u => u.ShortUrl)
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.GoogleId)
                .IsUnique();

            // RefreshToken → User FK
            modelBuilder.Entity<RefreshTokenEntity>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // RefreshToken: index on HashedToken for fast lookup
            modelBuilder.Entity<RefreshTokenEntity>()
                .HasIndex(r => r.HashedToken)
                .IsUnique();
        }
    }
}