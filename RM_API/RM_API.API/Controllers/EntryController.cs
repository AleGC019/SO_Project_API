using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM_API.Core.Models.EntryModels;
using RM_API.Service.Services.Interfaces;

namespace RM_API.API.Controllers;

[Authorize]
[ApiController]
[Route("api/entry/")]
public class EntryController : ControllerBase
{
    private readonly IEntryService _entryService;

    public EntryController(IEntryService entryService)
    {
        _entryService = entryService;
    }

    [Authorize(Policy = "Security")]
    [HttpGet("allEntries")]
    public async Task<IActionResult> GetAllEntries()
    {
        var response = await _entryService.GetAllEntries();
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }

    [Authorize("Security")]
    [HttpPost("requestEntry")]
    public async Task<IActionResult> RequestEntry([FromBody] RequireEntry entryRequest)
    {
        var response = await _entryService.RequestEntry(entryRequest);
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }
}