using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.AuthModels;
using RM_API.Core.Models.UserModels;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services.Interfaces;
using RM_API.Service.Tools;

namespace RM_API.Service.Services;

public class UserService : IUserService
{
    private readonly IRoleService _roleService;
    private readonly IUserRepository _userRepository;

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
        var roleResponse = await _roleService.GetOrCreateDefaultRole();

        // If the role doesn't exist, return the error message
        if (!roleResponse.Success)
            return new ResponseModel(false, roleResponse.Message);

        // Cast the role to Role
        var defaultRole = (Role)roleResponse.Data!;

        // Hash the password
        var hashedPassword = PasswordHelper.HashPassword(model.password);

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

    public async Task<ResponseModel> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            return new ResponseModel(false, "Usuario no encontrado");

        UserResponseModel userResponse = new()
        {
            email = user.UserEmail,
            username = user.UserName,
            role = user.UserRole.RoleName.ToString()
        };

        return new ResponseModel(true, "Usuario encontrado", userResponse);
    }

    public async Task<ResponseModel> GetUserByGuid(Guid guid)
    {
        var user = await _userRepository.GetByIdAsync(guid);
        if (user == null)
            return new ResponseModel(false, "Usuario no encontrado");

        UserResponseModel userResponse = new()
        {
            email = user.UserEmail,
            username = user.UserName,
            role = user.UserRole.RoleName.ToString()
        };

        return new ResponseModel(true, "Usuario encontrado", userResponse);
    }

    public async Task<ResponseModel> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();

        if (users.Count == 0)
            return new ResponseModel(false, "No hay usuarios registrados");

        List<UserResponseModel> userResponses = new();

        foreach (var user in users)
            userResponses.Add(new UserResponseModel
            {
                email = user.UserEmail,
                username = user.UserName,
                role = user.UserRole.RoleName.ToString()
            });

        return new ResponseModel(true, "Usuarios encontrados", userResponses);
    }

    public async Task<ResponseModel> UpdateUser(User user)
    {
        try
        {
            await _userRepository.UpdateAsync(user);
            return new ResponseModel(true, "Usuario actualizado exitosamente");
        }
        catch (Exception e)
        {
            return new ResponseModel(false, e.Message);
        }
    }

    public async Task<ResponseModel> DeactivateUser(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        Console.WriteLine(user);

        if (user == null)
            return new ResponseModel(false, "Usuario no encontrado");

        user.Permissions?.Clear();
        user.UserHouse = null;
        user.IsActive = false;

        Console.WriteLine("About to update");

        await _userRepository.UpdateAsync(user);

        Console.WriteLine("Updated");

        return new ResponseModel(true, "Usuario desactivado exitosamente");
    }
}