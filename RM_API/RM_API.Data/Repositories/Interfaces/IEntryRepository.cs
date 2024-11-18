using RM_API.Core.Entities;

namespace RM_API.Data.Repositories.Interfaces;

public interface IEntryRepository
{
    Task<List<Entry>?> GetAllEntries();
    Task<Entry?> GetEntryById(Guid id);
    Task CreateEntry(Entry entry);
    Task UpdateEntry(Entry entry);
}