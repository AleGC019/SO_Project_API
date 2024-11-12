using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.HouseModels;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services.Interfaces;

namespace RM_API.Service.Services;

public class HouseService : IHouseService
{
    private readonly IHouseRepository _houseRepository;

    public HouseService(IHouseRepository houseRepository)
    {
        _houseRepository = houseRepository;
    }

    public async Task<ResponseModel> SaveHouse(NewHouseModel model)
    {
        try
        {
            House house = new()
            {
                HouseNumber = model.houseNumber,
                HouseAddress = model.address
            };

            await _houseRepository.SaveHouse(house);

            return new ResponseModel(success: true, message: "House added successfully");
        }
        catch (Exception e)
        {
            return new ResponseModel(success: false, message: e.Message);
        }
    }

    public async Task<ResponseModel> GetHouseById(Guid id)
    {
        var house = await _houseRepository.GetHouseById(id);
        return (house == null) ? new ResponseModel(success: false, message: "House not found") : new ResponseModel(success: true, message: "House found", data: house);
    }
}