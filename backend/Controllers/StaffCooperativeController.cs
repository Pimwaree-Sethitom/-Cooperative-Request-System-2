using System.Security.Claims;
using backend.Common;
using backend.DTOs.Cooperative;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Authorize(Roles = "staff")]
[Route("api/staff/cooperative")]
public class StaffCooperativeController : BaseController
{
    private readonly ICooperativeService _cooperativeService;

    public StaffCooperativeController(ICooperativeService cooperativeService)
    {
        _cooperativeService = cooperativeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status)
    {
        var result = await _cooperativeService.GetAllCooperativesAsync(status);
        return Success(result);
    }

    [HttpPut("{id}/review")]
    public async Task<IActionResult> Review(int id, [FromBody] ReviewCooperativeRequest request)
    {
        try
        {
            var staffId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _cooperativeService.ReviewCooperativeAsync(staffId, id, request);
            return Success(result, "cooperative request reviewed successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
