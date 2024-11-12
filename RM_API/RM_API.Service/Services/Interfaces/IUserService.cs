using RM_API.Core.Models;
using RM_API.Core.Models.AuthModels;

namespace RM_API.Service.Services.Interfaces;

public interface IUserService
{
    Task<ResponseModel> RegisterAsync(RegisterModel model);

    Task<ResponseModel> ValidateUser(string email, string password);
}