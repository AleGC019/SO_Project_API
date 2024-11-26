using Microsoft.EntityFrameworkCore;
using RM_API.Core.Entities;
using RM_API.Data.Repositories.Interfaces;

namespace RM_API.Data.Repositories;

public class EntryRepository : IEntryRepository
{
    private readonly ApplicationDbContext _context;

    public EntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Entry>?> GetAllEntries()
    {
        return await _context.Entries
            .ToListAsync();
    }

    public async Task<Entry?> GetEntryById(Guid id)
    {
        return await _context
            .Entries
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task CreateEntry(Entry entry)
    {
        await _context.Entries.AddAsync(entry);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEntry(Entry entry)
    {
        _context.Entries.Update(entry);
        await _context.SaveChangesAsync();
    }
}