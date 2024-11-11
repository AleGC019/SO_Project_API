using RM_API.Core.Entities;

namespace RM_API.Core.Interfaces.IRole;

public interface IRoleRepository
{
    Task<Role?> GetRoleById(Guid id);
}