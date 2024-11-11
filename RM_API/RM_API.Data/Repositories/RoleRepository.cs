using Microsoft.EntityFrameworkCore;
using RM_API.Core.Entities;
using RM_API.Core.Interfaces.IRole;

namespace RM_API.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;
    
    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Role?> GetRoleById(Guid id)
    {
        return await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
    }
}