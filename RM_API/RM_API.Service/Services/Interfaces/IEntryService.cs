using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.EntryModels;

namespace RM_API.Service.Services.Interfaces;

public interface IEntryService
{
    Task<ResponseModel> GetAllEntries();
    Task<ResponseModel> GetEntryById(Guid id);
    Task<ResponseModel> CreateEntry(Entry entry);
    Task<ResponseModel> UpdateEntry(Entry entry);
    Task<ResponseModel> RequestEntry(RequireEntry entryRequest);
}