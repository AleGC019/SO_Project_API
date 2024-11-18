using RM_API.Core.Models;
using RM_API.Core.Models.HouseModels;

namespace RM_API.Service.Services.Interfaces;

public interface IHouseService
{
    Task<ResponseModel> SaveHouse(NewHouseModel model);
    Task<ResponseModel> GetHouseById(Guid id);
    Task<ResponseModel> GetHouseByHouseNumber(int number);
    Task<ResponseModel> GetAllHouses();
    Task<ResponseModel> AssignInhabitant(UserHouseModel model);
    Task<ResponseModel> RemoveInhabitant(UserHouseModel model);
}