using RM_API.Core.Models;
using RM_API.Core.Models.AuthModels;

namespace RM_API.Core.Interfaces.IUser
{
    public interface IUserService
    {
        Task<ResponseModel> RegisterAsync(RegisterModel model);
        Task LoginAsync(LoginModel model);
        
        Task<ResponseModel> ValidateUser(string email, string password);
    }
}