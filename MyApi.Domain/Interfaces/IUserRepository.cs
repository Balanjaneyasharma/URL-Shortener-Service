using MyApi.Domain.Models;

namespace MyApi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByGoogleIdAsync(string googleId);
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
}