using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM_API.Core.Entities;
using RM_API.Core.Models.HouseModels;
using RM_API.Service.Services.Interfaces;

namespace RM_API.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HouseController : ControllerBase
{
    private readonly IHouseService _houseService;


    public HouseController(IHouseService houseService)
    {
        _houseService = houseService;
    }

    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHouseById(Guid id)
    {
        var response = await _houseService.GetHouseById(id);
        if (!response.Success) return NotFound(response.Message);
        var house = (HouseResponseModel)response.Data!;
        return Ok(house);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("newHouse")]
    public async Task<IActionResult> AddHouse([FromBody] NewHouseModel request)
    {
        var response = await _houseService.SaveHouse(request);
        if (!response.Success) return Conflict(response.Message);
        return Ok(response);
    }

    [Authorize(Policy = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllHouses()
    {
        var response = await _houseService.GetAllHouses();
        if (!response.Success) return NotFound(response.Message);
        var houses = (List<HouseResponseModel>)response.Data!;
        return Ok(houses);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpPost("linkInhabitant")]
    public async Task<IActionResult> AppendUserToHouse([FromBody] AppendUserToHouseModel request)
    {
        var response = await _houseService.AssignInhabitant(request);
        if (!response.Success) return Conflict(response.Message);
        return Ok(response);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet("houseNumber/{number}")]
    public async Task<IActionResult> GetHouseByHouseNumber(int number)
    {
        var response = await _houseService.GetHouseByHouseNumber(number);
        if (!response.Success) return NotFound(response.Message);
        var house = (HouseResponseModel)response.Data!;
        return Ok(house);
    }
}