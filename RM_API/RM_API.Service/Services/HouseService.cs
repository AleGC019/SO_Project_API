using Microsoft.Identity.Client;
using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.HouseModels;
using RM_API.Core.Models.UserModels;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services.Interfaces;

namespace RM_API.Service.Services;

public class HouseService : IHouseService
{
    private readonly IHouseRepository _houseRepository;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public HouseService(IHouseRepository houseRepository, IUserService userService, IUserRepository userRepository)
    {
        _houseRepository = houseRepository;
        _userService = userService;
        _userRepository = userRepository;
    }

    public async Task<ResponseModel> SaveHouse(NewHouseModel model)
    {
        if (GetHouseByHouseNumber(model.houseNumber).Result.Success)
            return new ResponseModel(false, "House number already exists");

        try
        {
            House house = new()
            {
                HouseNumber = model.houseNumber,
                HouseAddress = model.address
            };

            await _houseRepository.SaveHouse(house);

            return new ResponseModel(true, "House added successfully");
        }
        catch (Exception e)
        {
            return new ResponseModel(false, e.Message);
        }
    }

    public async Task<ResponseModel> GetHouseById(Guid id)
    {
        var house = await _houseRepository.GetHouseById(id);

        if (house == null)
            return new ResponseModel(false, "House not found");

        HouseResponseModel response = new()
        {
            HouseNumber = house.HouseNumber,
            Address = house.HouseAddress,
            inhabitants = house.Inhabitants
                .Select(i => new UserResponseModel { email = i.UserEmail, username = i.UserName }).ToList()
        };

        return new ResponseModel(true, "House found", response);
    }

    public async Task<ResponseModel> GetHouseByHouseNumber(int number)
    {
        var house = await _houseRepository.GetHouseByHouseNumber(number);

        if (house == null)
            return new ResponseModel(false, "House not found");

        HouseResponseModel response = new()
        {
            HouseNumber = house.HouseNumber,
            Address = house.HouseAddress,
            inhabitants = house.Inhabitants
                .Select(i => new UserResponseModel { email = i.UserEmail, username = i.UserName }).ToList()
        };

        return new ResponseModel(true, "House found", response);
    }

    public async Task<ResponseModel> GetAllHouses()
    {
        var houses = await _houseRepository.GetAllHouses();

        if (houses.Count == 0)
            return new ResponseModel(false, "No houses found");

        List<HouseResponseModel> response = [];
        response.AddRange(houses.Select(house => new HouseResponseModel
        {
            HouseNumber = house!.HouseNumber, Address = house.HouseAddress,
            inhabitants = house.Inhabitants!
                .Select(i => new UserResponseModel { email = i.UserEmail, username = i.UserName }).ToList()
        }));

        return new ResponseModel(true, "Houses found", response);
    }

    public async Task<ResponseModel> AssignInhabitant(UserHouseModel request)
    {
        var email = request.UserEmail;
        var houseNumber = request.HouseNumber;

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            return new ResponseModel(false, "User not found");

        var houseSearch = await _houseRepository.GetHouseByHouseNumber(houseNumber);

        if (houseSearch == null)
            return new ResponseModel(false, "House not found");

        if (user.UserHouse != null && user.UserHouse.Equals(houseSearch))
            return new ResponseModel(false, "User already assigned to this house");

        if (user.UserHouse != null)
            return new ResponseModel(false, "User already assigned to an existing house. Remove user from house first");

        try
        {
            user.UserHouse = houseSearch;
            await _userService.UpdateUser(user);

            return new ResponseModel(true, "User assigned to house successfully");
        }
        catch (Exception e)
        {
            return new ResponseModel(false, e.Message);
        }
    }

    public async Task<ResponseModel> RemoveInhabitant(UserHouseModel model)
    {
        var user = await _userRepository.GetByEmailAsync(model.UserEmail);

        if (user == null)
            return new ResponseModel(false, "User not found");
        
        if(user.UserHouse == null)
            return new ResponseModel(false, "There is no house assigned to this user");

        var houseSearch = await _houseRepository.GetHouseByHouseNumber(model.HouseNumber);

        if (houseSearch == null)
            return new ResponseModel(false, "House not found");

        if (!user.UserHouse.Equals(houseSearch))
            return new ResponseModel(false, "User not assigned to this house");

        try
        {
            user.UserHouse = null;
            await _userService.UpdateUser(user);

            return new ResponseModel(true, "User removed from house successfully");
        }
        catch (Exception e)
        {
            return new ResponseModel(false, e.Message);
        }
    }
}