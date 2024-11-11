using RM_API.Core.Entities;

namespace RM_API.Core.Interfaces.IRole;

public interface IRoleService
{
    Task<Role?> GetRoleById(Guid id);
}