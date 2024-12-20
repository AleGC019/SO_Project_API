using RM_API.Core.Entities;

namespace RM_API.Data.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleById(Guid id);
    Task AddAsync(Role role);

    Task<Role?> GetRoleByName(RoleName name);
}