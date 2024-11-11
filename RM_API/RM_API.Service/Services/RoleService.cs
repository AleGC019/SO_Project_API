using RM_API.Core.Entities;
using RM_API.Core.Interfaces.IRole;

namespace RM_API.Service.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task<Role?> GetRoleById(Guid id)
    {
        return await _roleRepository.GetRoleById(id);
    }
}