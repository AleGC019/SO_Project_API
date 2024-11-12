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

    // TODO: Restrict this to admin only
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHouseById(Guid id)
    {
        var response = await _houseService.GetHouseById(id);
        if (!response.Success) return NotFound(response.Message);
        var house = (House)response.Data!;
        return Ok(house);
    }

    // TODO: Restrict this to admin only
    [Authorize(Policy = "Admin")]
    [HttpPost("add")]
    public async Task<IActionResult> AddHouse([FromBody] NewHouseModel request)
    {
        var response = await _houseService.SaveHouse(request);
        if (!response.Success) return Conflict(response.Message);
        return Ok(response);
    }
}