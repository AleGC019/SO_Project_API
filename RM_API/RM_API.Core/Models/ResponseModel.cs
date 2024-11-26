namespace RM_API.Core.Models;

public class ResponseModel
{
    public ResponseModel(bool success, string? message, object? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public ResponseModel(bool success, string? message)
    {
        Success = success;
        Message = message;
    }

    public ResponseModel(bool success)
    {
        Success = success;
    }

    public ResponseModel()
    {
    }

    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}