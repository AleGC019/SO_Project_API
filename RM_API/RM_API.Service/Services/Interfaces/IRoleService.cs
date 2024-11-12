using RM_API.Core.Entities;
using RM_API.Core.Models;

namespace RM_API.Service.Services.Interfaces;

public interface IRoleService
{
    Task<ResponseModel> GetRoleById(Guid id);
    Task<ResponseModel> CreateNewRole(Role role);
    Task<ResponseModel> GetOrCreateDefaultRole();
}