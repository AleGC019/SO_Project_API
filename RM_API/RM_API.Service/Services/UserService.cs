// Ubicación: RM_API.Service.Services.UserService.cs

using RM_API.Core.Entities;
using RM_API.Core.Interfaces;
using RM_API.Core.Models;
using RM_API.Service.Utils;
using Microsoft.Extensions.Configuration;
using RM_API.Core.Interfaces.IUser;
using RM_API.Core.Models.AuthModels;

namespace RM_API.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ResponseModel> RegisterAsync(RegisterModel model)
        {
            // Verificar si el usuario ya existe
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(model.email);

                if (existingUser != null)
                    return new ResponseModel(false, "El usuario con el correo ingresado ya existe");
            }
            catch (Exception ex)
            {
                return new ResponseModel(false, ex.Message, null);
            }

            // // Obtener el rol predeterminado desde la configuración (si lo necesitas)
            var defaultRoleName = RoleName.RES;

            // Get user age by using his/her birthdate up to this day. Remember to have day and month in mind.
            var age = DateTime.UtcNow.Year - DateOnly.Parse(model.BirthDate).Year;

            // Crear el nuevo usuario y encriptar la contraseña
            var hashedPassword = PasswordHelper.HashPassword(model.Password);

            var newUser = new User
            {
                UserName = model.username,
                UserEmail = model.email,
                UserPassword = hashedPassword,
                UserAge = age,
                UserRole = new Role() { RoleName = defaultRoleName }
            };

            await _userRepository.AddAsync(newUser);
            
            return new ResponseModel(true, "Usuario registrado exitosamente");
        }

        public Task LoginAsync(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> ValidateUser(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            
            if (user == null)
                return new ResponseModel(false, "Usuario no encontrado");
            
            if (!PasswordHelper.VerifyPassword(password, user.UserPassword))
                return new ResponseModel(false, "Contraseña incorrecta");
            
            return new ResponseModel(true, "Usuario encontrado", user);
        }

        // Register user service
    }
}