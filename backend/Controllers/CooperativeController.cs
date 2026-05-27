using System.Security.Claims;
using backend.Common;
using backend.DTOs.Cooperative;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Authorize(Roles = "public")]
public class CooperativeController : BaseController
{
    private readonly ICooperativeService _cooperativeService;

    public CooperativeController(ICooperativeService cooperativeService)
    {
        _cooperativeService = cooperativeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCooperativeRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _cooperativeService.CreateCooperativeAsync(userId, request);
            return Created(result, "cooperative request submitted successfully");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _cooperativeService.GetMyCooperativesAsync(userId);
        return Success(result);
    }
}
