using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.EntryModels;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services.Interfaces;

namespace RM_API.Service.Services;

public class EntryService : IEntryService
{
    private readonly IEntryRepository _entryRepository;
    private readonly IPermitRepository _permitRepository;
    private readonly IPermitService _permitService;

    public EntryService(IEntryRepository entryRepository, IPermitRepository permitRepository,
        IPermitService permitService)
    {
        _entryRepository = entryRepository;
        _permitRepository = permitRepository;
        _permitService = permitService;
    }

    public async Task<ResponseModel> GetAllEntries()
    {
        var entries = await _entryRepository.GetAllEntries();

        if (entries.Count > 0)
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
        // Check if the user has any permit to enter the house
        var permits = await _permitRepository.GetPermitByHomeAndUser(entryRequest.UserId, entryRequest.HouseId);

        if (permits == null || permits.Count == 0)
            return new ResponseModel(false, "No valid permit found for the user to enter the house");

        // Check validation of the permits
        foreach (var permit in permits)
        {
            await _permitService.CheckPermit(permit);
        }

        var validPermits = permits.Where(p => p.Status).ToList();

        // Check if in the validPermits there is any permit that is valid for the current time, that means, current time is between the start and end date of the permit
        var validPermit =
            validPermits.FirstOrDefault(p => entryRequest.Date >= p.StartDate && entryRequest.Date <= p.EndDate);

        if (validPermit == null)
            return new ResponseModel(false, "No valid permit found for the user to enter the house at this moment.");
        
        // Validating terminal type
        if (Enum.TryParse(entryRequest.Terminal, out TerminalType terminalType))
        {
            entryRequest.Terminal = terminalType.ToString();
        }
        else
        {
            return new ResponseModel(false, "Invalid terminal type");
        }
        
        // Create the entry
        var entry = new Entry
        {
            EntryComment = entryRequest.Description,
            EntryTimestamp = DateTime.UtcNow,
            EntryTerminal = terminalType,
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