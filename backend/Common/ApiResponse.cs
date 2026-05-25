namespace backend.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "success") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message) =>
        new() { Success = false, Message = message, Data = default };

    public static ApiResponse<T> ValidationFail(Dictionary<string, string[]> errors) =>
        new() { Success = false, Message = "validation failed", Errors = errors };
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Ok(string message = "success") =>
        new() { Success = true, Message = message, Data = null };

    public new static ApiResponse Fail(string message) =>
        new() { Success = false, Message = message, Data = null };

    public new static ApiResponse ValidationFail(Dictionary<string, string[]> errors) =>
        new() { Success = false, Message = "validation failed", Errors = errors };
}
