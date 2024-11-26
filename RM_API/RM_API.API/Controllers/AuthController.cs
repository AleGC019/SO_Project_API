using Microsoft.AspNetCore.Mvc;
using RM_API.API.Utils;
using RM_API.API.Utils.JWT;
using RM_API.Core.Entities;
using RM_API.Core.Models;
using RM_API.Core.Models.AuthModels;
using RM_API.Service.Services.Interfaces;

namespace RM_API.API.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;

    public AuthController(IUserService userService, JwtTokenGenerator jwtTokenGenerator, IRoleService roleService)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _roleService = roleService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.RegisterAsync(model);

        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // If the model is not valid, return a bad request
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Validate the user
        var validation = await _userService.ValidateUser(model.Email, model.Password);

        // If the validation is not successful, return an unauthorized response
        if (!validation.Success) return Unauthorized(validation.Message);

        // Cast the data to User
        var user = (User)validation.Data!;

        // Get the user's role
        var roleResponse = await _roleService.GetRoleById(user.RoleId);

        // If the role doesn't exist, return a bad request
        if (!roleResponse.Success) return BadRequest("User may not be authorized anymore");

        // Cast the role to Role
        user.UserRole = (Role)roleResponse.Data!;

        // Generate the JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return Ok(new { token });
    }
}