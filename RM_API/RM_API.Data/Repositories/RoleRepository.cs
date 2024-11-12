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

    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }

    public async Task<Role?> GetRoleByName(RoleName name)
    {
        return await _context.Roles.SingleOrDefaultAsync(r => r.RoleName == name);
    }
}