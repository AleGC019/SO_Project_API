using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.EntryModels;
using RM_API.Core.Models.HouseModels;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services.Interfaces;

namespace RM_API.Service.Services;

public class EntryService : IEntryService
{
    private readonly IEntryRepository _entryRepository;
    private readonly IPermitRepository _permitRepository;

    public EntryService(IEntryRepository entryRepository, IPermitRepository permitRepository)
    {
        _entryRepository = entryRepository;
        _permitRepository = permitRepository;
    }

    public async Task<ResponseModel> GetAllEntries()
    {
        var entries = await _entryRepository.GetAllEntries();

        if (entries != null)
        {
            var response = entries.Select(e => new EntryResponseModel
            {
                EntryComment = e.EntryComment ?? "",
                EntryTimestamp = e.EntryTimestamp,
                EntryTerminal = e.EntryTerminal.ToString(),
                PermissionId = e.PermissionId.ToString()
            }).ToList();

            return new ResponseModel(true, "Entries found", response);
        }

        return new ResponseModel(false, "No entries found");
    }

    public async Task<ResponseModel> GetEntryById(Guid id)
    {
        var entry = await _entryRepository.GetEntryById(id);

        if (entry == null)
            return new ResponseModel(false, "Entry not found");

        EntryResponseModel response = new()
        {
            EntryComment = entry.EntryComment ?? "",
            EntryTimestamp = entry.EntryTimestamp,
            EntryTerminal = entry.EntryTerminal.ToString(),
            PermissionId = entry.PermissionId.ToString()
        };

        return new ResponseModel(true, "Entry found", response);
    }

    public async Task<ResponseModel> CreateEntry(Entry entry)
    {
        try
        {
            await _entryRepository.CreateEntry(entry);
            return new ResponseModel(true, "Entry added successfully");
        }
        catch (Exception e)
        {
            return new ResponseModel(false, e.Message);
        }
    }

    public async Task<ResponseModel> UpdateEntry(Entry entry)
    {
        try
        {
            await _entryRepository.UpdateEntry(entry);
            return new ResponseModel(true, "Entry updated successfully");
        }
        catch (Exception e)
        {
            return new ResponseModel(false, e.Message);
        }
    }

    public async Task<ResponseModel> RequestEntry(RequireEntry entryRequest)
    {
        // Check if the user has a valid permit for the house
        var validPermit = await _permitRepository.GetValidPermit(entryRequest.UserId, entryRequest.HouseId);
        if (validPermit == null)
        {
            return new ResponseModel(false, "No valid permit found for the user to enter the house");
        }
        
        // Check permit validation from now with the start and end date
        if (entryRequest.Date < validPermit.StartDate || entryRequest.Date > validPermit.EndDate)
        {
            // Check if the permit has not yet been deactivated. If so, update permit in the database
            if (validPermit.Status)
            {
                validPermit.Status = false;
                await _permitRepository.UpdatePermit(validPermit);
            }
            return new ResponseModel(false, "Permit is not valid for the current time");
        }

        // Create the entry
        var entry = new Entry
        {
            EntryComment = entryRequest.Description,
            EntryTimestamp = DateTime.UtcNow,
            EntryTerminal = entryRequest.Terminal,
            PermissionId = validPermit.Id
        };

        try
        {
            await _entryRepository.CreateEntry(entry);
            return new ResponseModel(true, "Entry request successful");
        }
        catch (Exception e)
        {
            return new ResponseModel(false, e.Message);
        }
    }
}