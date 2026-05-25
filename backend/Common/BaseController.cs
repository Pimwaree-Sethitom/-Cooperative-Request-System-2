using Microsoft.AspNetCore.Mvc;

namespace backend.Common;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Success<T>(T data, string message = "success") =>
        Ok(ApiResponse<T>.Ok(data, message));

    protected IActionResult Created<T>(T data, string message = "created successfully") =>
        StatusCode(201, ApiResponse<T>.Ok(data, message));

    protected IActionResult NoContent(string message = "deleted successfully") =>
        Ok(ApiResponse.Ok(message));

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(ApiResponse.Fail(message));

    protected IActionResult NotFound(string message) =>
        base.NotFound(ApiResponse.Fail(message));

    protected IActionResult Conflict(string message) =>
        StatusCode(409, ApiResponse.Fail(message));

    protected IActionResult Unauthorized(string message) =>
        base.Unauthorized(ApiResponse.Fail(message));

    protected IActionResult Forbidden(string message) =>
        StatusCode(403, ApiResponse.Fail(message));

    protected IActionResult ServerError(string message = "internal server error") =>
        StatusCode(500, ApiResponse.Fail(message));
}
