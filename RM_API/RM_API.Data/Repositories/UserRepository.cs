// Ubicación: RM_API.Data.Repositories.UserRepository.cs

using Microsoft.EntityFrameworkCore;
using RM_API.Core.Entities;
using RM_API.Core.Interfaces;
using System.Threading.Tasks;

namespace RM_API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserEmail == email);
        }
    }
}