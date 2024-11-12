using RM_API.Core.Entities;
using RM_API.Core.Interfaces;
using RM_API.Core.Interfaces.IRole;
using RM_API.Core.Models;
using RM_API.Core.Models.AuthModels;
using RM_API.Service.Services.Interfaces;
using RM_API.Service.Utils;

namespace RM_API.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;

    public UserService(IUserRepository userRepository, IRoleService roleService)
    {
        _userRepository = userRepository;
        _roleService = roleService;
    }

    public async Task<ResponseModel> RegisterAsync(RegisterModel model)
    {
        // Verify if the user already exists
        var existingUser = await _userRepository.GetByEmailAsync(model.email);

        // If the user already exists, return the error message
        if (existingUser != null)
            return new ResponseModel(false, "El usuario con el correo ingresado ya existe");

        // Obtain the default role. If it doesn't exist, create it
        var roleResponse = await _roleService.GetOrCreateRoleByRoleName(RoleName.RES);

        // If the role doesn't exist, return the error message
        if (!roleResponse.Success)
            return new ResponseModel(false, roleResponse.Message);

        // Cast the role to Role
        var defaultRole = (Role)roleResponse.Data!;

        // Hash the password
        var hashedPassword = PasswordHelper.HashPassword(model.Password);

        // Create the new user
        var newUser = new User
        {
            UserName = model.username,
            UserEmail = model.email,
            UserPassword = hashedPassword,
            UserRole = defaultRole
        };

        await _userRepository.AddAsync(newUser);

        return new ResponseModel(true, "Usuario registrado exitosamente");
    }

    public async Task<ResponseModel> ValidateUser(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            return new ResponseModel(false, "Usuario no encontrado");

        return !PasswordHelper.VerifyPassword(password, user.UserPassword)
            ? new ResponseModel(false, "Contraseña incorrecta")
            : new ResponseModel(true, "Usuario encontrado", user);
    }
}