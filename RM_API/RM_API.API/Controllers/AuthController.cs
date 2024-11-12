using Microsoft.AspNetCore.Mvc;
using RM_API.API.Utils;
using RM_API.Core.Entities;
using RM_API.Core.Interfaces.IRole;
using RM_API.Core.Models;
using RM_API.Core.Models.AuthModels;
using RM_API.Service.Services.Interfaces;
using RM_API.Service.Tools;

namespace RM_API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IRoleService _roleService;
    private readonly TimeZoneTool _timeZoneTool;
    private readonly IUserService _userService;

    public AuthController(IUserService userService, JwtTokenGenerator jwtTokenGenerator, IRoleService roleService,
        TimeZoneTool timeZoneTool)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _roleService = roleService;
        _timeZoneTool = timeZoneTool;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var validation = await _userService.ValidateUser(model.Email, model.Password);

        if (!validation.Success) return Unauthorized(validation.Message);

        var user = (User)validation.Data!;

        var role = await _roleService.GetRoleById(user.RoleId);

        if (role == null) return BadRequest("User has no role assigned");

        // Generate the JWT token
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, role.ToString()!);

        return Ok(new { token });
    }


    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.RegisterAsync(model);

        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response.Message);
    }
}