using Microsoft.EntityFrameworkCore;
using RM_API.Core.Entities;
using RM_API.Data.Repositories.Interfaces;

namespace RM_API.Data.Repositories;

public class PermitRepository : IPermitRepository
{
    private readonly ApplicationDbContext _context;

    public PermitRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Permission>?> GetAllPermits()
    {
        return await _context.Permissions.ToListAsync();
    }

    public async Task CreatePermit(Permission permission)
    {
        await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePermit(Guid permitId)
    {
        var permit = await _context.Permissions.FirstOrDefaultAsync(p => p.Id == permitId);
        if (permit == null)
            return;
        _context.Permissions.Remove(permit);
        await _context.SaveChangesAsync();
    }

    public Task<Permission?> GetPermitById(Guid permit)
    {
        return _context.Permissions.FirstOrDefaultAsync(p => p.Id == permit);
    }

    public async Task UpdatePermit(Permission permission)
    {
        _context.Permissions.Update(permission);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Permission>?> GetPermitsByHouseId(Guid houseId)
    {
        return await _context.Permissions.Where(p => p.HouseId == houseId).ToListAsync();
    }

    public async Task<Permission?> GetValidPermit(Guid userId, Guid houseId)
    {
        return await _context.Permissions.FirstOrDefaultAsync(p =>
            p.UserId == userId && p.HouseId == houseId && p.Status == true);
    }
}