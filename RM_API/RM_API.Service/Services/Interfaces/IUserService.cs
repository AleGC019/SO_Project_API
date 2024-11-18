using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.AuthModels;

namespace RM_API.Service.Services.Interfaces;

public interface IUserService
{
    Task<ResponseModel> RegisterAsync(RegisterModel model);

    Task<ResponseModel> ValidateUser(string email, string password);
    Task<ResponseModel> GetUserByEmail(string email);
    Task<ResponseModel> GetUserByGuid(Guid guid);
    Task<ResponseModel> GetAllUsers();
    Task<ResponseModel> UpdateUser(User user);
    Task<ResponseModel> DeactivateUser(string email);
}