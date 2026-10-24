using Microsoft.EntityFrameworkCore;
using MyApi.Domain.Interfaces;
using MyApi.Domain.Models;
using MyApi.Infrastructure.Entities;
using MyApi.Infrastructure.Persistence;

namespace MyApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.GoogleId == googleId);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<User> CreateAsync(User user)
    {
        var entity = new UserEntity
        {
            Email = user.Email,
            GoogleId = user.GoogleId,
            Name = user.Name,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        user.Id = entity.Id;
        return user;
    }

    private static User MapToDomain(UserEntity entity) => new()
    {
        Id = entity.Id,
        Email = entity.Email,
        GoogleId = entity.GoogleId,
        Name = entity.Name,
        CreatedAt = entity.CreatedAt
    };
}