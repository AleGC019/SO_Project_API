using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.Permission;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services.Interfaces;
using RM_API.Service.Tools;

namespace RM_API.Service.Services;

public class PermitService : IPermitService
{
    private readonly IHouseService _houseService;
    private readonly IPermitRepository _permitRepository;
    private readonly IUserService _userService;
    private readonly TimeZoneTool _timeZoneTool;

    public PermitService(IPermitRepository permitRepository, IHouseService houseService, IUserService userService,
        TimeZoneTool timeZoneTool)
    {
        _permitRepository = permitRepository;
        _houseService = houseService;
        _userService = userService;
        _timeZoneTool = timeZoneTool;
    }

    public async Task CheckPermit(Permission permit)
    {
        if (permit.Status
            &&
            (permit.StartDate > DateTime.UtcNow || permit.EndDate < DateTime.UtcNow))
        {
            permit.Status = false;
            await _permitRepository.UpdatePermit(permit);
        }

        if (!permit.Status
            &&
            (permit.StartDate < DateTime.UtcNow && permit.EndDate > DateTime.UtcNow))
        {
            permit.Status = true;
            await _permitRepository.UpdatePermit(permit);
        }
    }

    public async Task<ResponseModel> GetAllPermits()
    {
        try
        {
            var permits = await _permitRepository.GetAllPermits();

            if (permits != null && permits.Count == 0)
                return new ResponseModel
                {
                    Success = false,
                    Message = "No permits found"
                };

            List<PermissionResponse> permissionModels = new();

            if (permits != null)
                foreach (var permit in permits)
                {
                    CheckPermit(permit).Wait();
                    permissionModels.Add(new PermissionResponse
                    {
                        PermitId = permit.Id,
                        StartDate = _timeZoneTool.ConvertUtcToAppTimeZone(permit.StartDate),
                        EndDate = _timeZoneTool.ConvertUtcToAppTimeZone(permit.EndDate),
                        HouseId = permit.HouseId,
                        UserId = permit.UserId,
                        Status = permit.Status ? "Active" : "Inactive"
                    });
                }

            return new ResponseModel
            {
                Success = true,
                Message = "Permits found",
                Data = permissionModels
            };
        }
        catch (Exception e)
        {
            return new ResponseModel
            {
                Success = false,
                Message = e.Message
            };
        }
    }

    public async Task<ResponseModel> CreatePermit(PermissionRequest permissionRequest)
    {
        // Permit request needs to be validated on certain points:
        // 1. Start date should be less than end date
        // 2. Start date should be equal or greater than current date
        // 3. HouseId should be valid
        // 4. UserId should be valid
        // 5. Status should be true if the permit is active in the current time
        // 6. Permit should not overlap with existing permits
        // 7. Permit should not be in the past
        // If any of the above conditions are not met, return a response model with success as false and message as the error message

        permissionRequest.StartDate = _timeZoneTool.ConvertAppTimeZoneToUtc(permissionRequest.StartDate);
        permissionRequest.EndDate = _timeZoneTool.ConvertAppTimeZoneToUtc(permissionRequest.EndDate);

        // Validation checks
        if (permissionRequest.StartDate >= permissionRequest.EndDate)
            return new ResponseModel
            {
                Success = false,
                Message = "Start date must happen before the end date."
            };

        if (permissionRequest.StartDate < DateTime.UtcNow)
            return new ResponseModel
            {
                Success = false,
                Message = "Start date should happen from now onwards, but you have selected a past date."
            };

        var house = await _houseService.GetHouseById(permissionRequest.HouseId);

        if (!house.Success)
            return new ResponseModel
            {
                Success = false,
                Message = "Invalid House identifier"
            };

        var user = await _userService.GetUserByGuid(permissionRequest.UserId);

        if (!user.Success)
            return new ResponseModel
            {
                Success = false,
                Message = "Invalid User identifier"
            };

        var existingPermits = await _permitRepository.GetPermitByHomeAndUser(permissionRequest.UserId,
            permissionRequest.HouseId);

        var overlappingPermit = existingPermits?
            .FirstOrDefault(
                p =>
                    (permissionRequest.StartDate >= p.StartDate && permissionRequest.StartDate <= p.EndDate)
                    ||
                    (permissionRequest.EndDate >= p.StartDate && permissionRequest.EndDate <= p.EndDate)
            );

        if (overlappingPermit != null)
        {
            var permissionResponse = new PermissionResponse
            {
                PermitId = overlappingPermit.Id,
                StartDate = _timeZoneTool.ConvertUtcToAppTimeZone(overlappingPermit.StartDate),
                EndDate = _timeZoneTool.ConvertUtcToAppTimeZone(overlappingPermit.EndDate),
                HouseId = overlappingPermit.HouseId,
                UserId = overlappingPermit.UserId,
                Status = overlappingPermit.Status ? "Active" : "Inactive"
            };

            return new ResponseModel
            {
                Success = false,
                Message = "Requested new permit overlaps with an existing permit for the same user and house.",
                Data = permissionResponse // Include the overlapping permit in the response
            };
        }

        try
        {
            var permit = new Permission
            {
                StartDate = permissionRequest.StartDate,
                EndDate = permissionRequest.EndDate,
                HouseId = permissionRequest.HouseId,
                UserId = permissionRequest.UserId,
                Status =
                    (permissionRequest.StartDate <= DateTime.UtcNow && permissionRequest.EndDate >= DateTime.UtcNow)
            };
            await _permitRepository.CreatePermit(permit);
            return new ResponseModel
            {
                Success = true,
                Message = "Permit created successfully"
            };
        }
        catch (Exception e)
        {
            return new ResponseModel
            {
                Success = false,
                Message = e.Message
            };
        }
    }

    public async Task<ResponseModel> DeletePermit(Guid permitId)
    {
        try
        {
            var permit = await _permitRepository.GetPermitById(permitId);
            if (permit == null)
                return new ResponseModel
                {
                    Success = false,
                    Message = "Permit not found"
                };

            permit.IsActive = false;
            permit.Status = false;

            await _permitRepository.UpdatePermit(permit);

            return new ResponseModel
            {
                Success = true,
                Message = "Permit deleted successfully"
            };
        }
        catch (Exception e)
        {
            return new ResponseModel
            {
                Success = false,
                Message = e.Message
            };
        }
    }

    public async Task<ResponseModel> GetMyPermits(Guid userId)
    {
        try
        {
            var permits = await _permitRepository.GetPermitsByUserId(userId);

            if (permits != null && permits.Count == 0)
                return new ResponseModel
                {
                    Success = false,
                    Message = "No permits found"
                };

            List<PermissionResponse> permissionModels = new();

            if (permits != null)
                foreach (var permit in permits)
                {
                    CheckPermit(permit).Wait();

                    permissionModels.Add(new PermissionResponse
                    {
                        PermitId = permit.Id,
                        StartDate = _timeZoneTool.ConvertUtcToAppTimeZone(permit.StartDate),
                        EndDate = _timeZoneTool.ConvertUtcToAppTimeZone(permit.EndDate),
                        HouseId = permit.HouseId,
                        UserId = permit.UserId,
                        Status = permit.Status ? "Active" : "Inactive"
                    });
                }

            return new ResponseModel
            {
                Success = true,
                Message = "Permits found",
                Data = permissionModels
            };
        }
        catch (Exception e)
        {
            return new ResponseModel
            {
                Success = false,
                Message = e.Message
            };
        }
    }
}