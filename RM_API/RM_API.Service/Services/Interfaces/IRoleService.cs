using RM_API.Core.Entities;
using RM_API.Core.Models;

namespace RM_API.Core.Interfaces.IRole;

public interface IRoleService
{
    Task<Role?> GetRoleById(Guid id);
    Task<ResponseModel> CreateNewRole(Role role);
    Task<ResponseModel> GetOrCreateRoleByRoleName(RoleName name);
}