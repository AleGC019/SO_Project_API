using RM_API.Core.Entities;
using RM_API.Core.Interfaces.IRole;
using RM_API.Core.Models;

namespace RM_API.Service.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ResponseModel> GetOrCreateRoleByRoleName(RoleName name)
    {
        var role = await _roleRepository.GetRoleByName(name);

        if (role != null)
            return new ResponseModel(
                true,
                "Role found",
                role
            );

        var newRole = new Role
        {
            RoleName = name
        };

        try
        {
            await _roleRepository.AddAsync(newRole);
            return new ResponseModel(
                true,
                "Role created and found!",
                newRole
            );
        }
        catch (Exception e)
        {
            return new ResponseModel(
                false,
                $"Failed to create role: {e.Message}"
            );
        }
    }

    public async Task<Role?> GetRoleById(Guid id)
    {
        return await _roleRepository.GetRoleById(id);
    }

    public async Task<ResponseModel> GetRoleByName(RoleName name)
    {
        var role = await _roleRepository.GetRoleByName(name);

        return role != null
            ? new ResponseModel(
                true,
                "Role found",
                role
            )
            : new ResponseModel(
                false,
                "Role not found"
            );
    }

    public async Task<ResponseModel> CreateNewRole(Role role)
    {
        try
        {
            await _roleRepository.AddAsync(role);
            return new ResponseModel(
                true,
                "Role added successfully",
                role
            );
        }
        catch (Exception ex)
        {
            return new ResponseModel(
                false,
                ex.Message
            );
        }
    }
}