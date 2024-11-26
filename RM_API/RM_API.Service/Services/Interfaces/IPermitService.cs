using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.Permission;

namespace RM_API.Service.Services.Interfaces;

public interface IPermitService
{
    Task<ResponseModel> GetAllPermits();
    Task<ResponseModel> CreatePermit(PermissionRequest permissionRequest);
    Task<ResponseModel> DeletePermit(Guid permitId);
    Task<ResponseModel> GetMyPermits(Guid userId);
    Task CheckPermit(Permission permit);
}