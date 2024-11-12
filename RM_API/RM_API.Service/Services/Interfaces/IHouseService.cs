using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.HouseModels;

namespace RM_API.Service.Services.Interfaces;

public interface IHouseService
{
    Task<ResponseModel> SaveHouse(NewHouseModel model);
    Task<ResponseModel> GetHouseById(Guid id);
}