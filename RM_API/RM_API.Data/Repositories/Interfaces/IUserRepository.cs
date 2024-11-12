using RM_API.Core.Entities;

namespace RM_API.Core.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}