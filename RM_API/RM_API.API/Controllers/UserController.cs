using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM_API.Service.Services.Interfaces;

namespace RM_API.API.Controllers;

[Authorize]
[ApiController]
[Route("api/user/")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("whoami")]
    public Task<IActionResult> WhoAmI()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        return Task.FromResult<IActionResult>(Ok(new { userId, userName, userRole }));
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("delete/{email}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        Console.WriteLine("ENTRANDO AL CONTROLADOR" + email);
        var response = await _userService.DeactivateUser(email);
        if (!response.Success) return NotFound(response.Message);
        return Ok(response);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet("getByEmail/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var response = await _userService.GetUserByEmail(email);
        if (!response.Success) return NotFound(response.Message);
        return Ok(response);
    }
    
    [Authorize("Admin")]
    [HttpGet("getById/{guid}")]
    public async Task<IActionResult> GetUserByGuid(Guid guid)
    {
        var response = await _userService.GetUserByGuid(guid);
        if (!response.Success) return NotFound(response.Message);
        return Ok(response);
    }
    
    [Authorize("Admin")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _userService.GetAllUsers();
        if (!response.Success) return NotFound(response.Message);
        return Ok(response);
    }
}