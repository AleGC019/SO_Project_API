using Microsoft.EntityFrameworkCore;
using RM_API.Core.Entities;
using RM_API.Data.Repositories.Interfaces;

namespace RM_API.Data.Repositories;

public class HouseRepository : IHouseRepository
{
    private readonly ApplicationDbContext _context;

    public HouseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SaveHouse(House house)
    {
        await _context.Houses.AddAsync(house);
        await _context.SaveChangesAsync();
    }

    public async Task<House?> GetHouseById(Guid id)
    {
        return await _context
            .Houses
            .Include(h => h.Inhabitants)
            .SingleOrDefaultAsync(h => h.Id == id);
    }

    public async Task<House?> GetHouseByHouseNumber(int houseNumber)
    {
        return await _context
            .Houses
            .Include(h => h.Inhabitants)
            .SingleOrDefaultAsync(h => h.HouseNumber == houseNumber);
    }

    public async Task<List<House>?> GetAllHouses()
    {
        return await _context.Houses
            .Include(h => h.Inhabitants)
            .ToListAsync();
    }

    public async Task UpdateHouse(House house)
    {
        _context.Houses.Update(house);
        await _context.SaveChangesAsync();
    }
}