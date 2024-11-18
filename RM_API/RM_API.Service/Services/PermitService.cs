using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.Permission;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services.Interfaces;

namespace RM_API.Service.Services;

public class PermitService : IPermitService
{
    private readonly IPermitRepository _permitRepository;
    private readonly IHouseService _houseService;
    private readonly IUserService _userService;

    public PermitService(IPermitRepository permitRepository, IHouseService houseService, IUserService userService)
    {
        _permitRepository = permitRepository;
        _houseService = houseService;
        _userService = userService;
    }

    public async Task<ResponseModel> GetAllPermits()
    {
        try
        {
            var permits = await _permitRepository.GetAllPermits();
            if (permits == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "No permits found"
                };
            }

            List<PermissionResponse> permissionModels = new();
            foreach (var permit in permits)
            {
                permissionModels.Add(new PermissionResponse
                {
                    PermitId = permit.Id,
                    StartDate = permit.StartDate,
                    EndDate = permit.EndDate,
                    HouseId = permit.HouseId,
                    UserId = permit.UserId,
                    Status = permit.Status
                });
            }

            return new ResponseModel
            {
                Success = true,
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
        // 5. Status should be true
        // 6. Permit should not overlap with existing permits
        // 7. Permit should not be in the past
        // If any of the above conditions are not met, return a response model with success as false and message as the error message

        // Validation checks
        if (permissionRequest.StartDate >= permissionRequest.EndDate)
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Start date should be less than end date"
            };
        }

        if (permissionRequest.StartDate < DateTime.UtcNow)
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Start date should be equal or greater than current date"
            };
        }

        var house = await _houseService.GetHouseById(permissionRequest.HouseId);
        if (house == null)
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Invalid HouseId"
            };
        }

        var user = await _userService.GetUserByGuid(permissionRequest.UserId);
        if (user == null)
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Invalid UserId"
            };
        }

        var existingPermits = await _permitRepository.GetPermitsByHouseId(permissionRequest.HouseId);
        if (existingPermits != null && existingPermits.Any(p =>
                p.StartDate < permissionRequest.EndDate && p.EndDate > permissionRequest.StartDate))
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Permit overlaps with existing permits"
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
                Status = true
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
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Permit not found"
                };
            }

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
}