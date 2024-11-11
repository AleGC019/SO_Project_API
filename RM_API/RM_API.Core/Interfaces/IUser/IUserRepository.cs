using RM_API.Core.Entities;
using RM_API.Core.Models;

namespace RM_API.Core.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}