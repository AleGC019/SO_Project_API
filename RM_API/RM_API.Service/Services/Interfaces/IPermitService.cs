using RM_API.Core.Models;
using RM_API.Core.Models.Permission;

namespace RM_API.Service.Services.Interfaces;

public interface IPermitService
{
    Task<ResponseModel> GetAllPermits();
    Task<ResponseModel> CreatePermit(PermissionRequest permissionRequest);
    Task<ResponseModel> DeletePermit(Guid permitId);
}