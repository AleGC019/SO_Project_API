using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM_API.Core.Models.Permission;
using RM_API.Service.Services.Interfaces;

namespace RM_API.API.Controllers;

[Authorize]
[ApiController]
[Route("api/permit/")]
public class PermitController: ControllerBase
{
    private readonly IPermitService _permitService;

    public PermitController(IPermitService permitService)
    {
        _permitService = permitService;
    }
    
    [Authorize(Policy = "Security")]
    [HttpGet("allPermits")]
    public async Task<IActionResult> GetAllPermits()
    {
        var response = await _permitService.GetAllPermits();
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }
    
    [Authorize(Policy = "Residents")]   
    [HttpPost("createPermit")]
    public async Task<IActionResult> CreatePermit([FromBody] PermissionRequest permit)
    {
        var response = await _permitService.CreatePermit(permit);
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }
    
    [Authorize(Policy = "Residents")]
    [HttpDelete("deletePermit/{permitId}")]
    public async Task<IActionResult> DeletePermit(Guid permitId)
    {
        var response = await _permitService.DeletePermit(permitId);
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }
}