using RM_API.Core.Entities;

namespace RM_API.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>?> GetAllAsync();
    Task UpdateAsync(User user);
}