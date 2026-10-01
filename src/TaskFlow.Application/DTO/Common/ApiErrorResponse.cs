namespace TaskFlow.Application.DTO.Common;

public class ApiErrorResponse
{
    public string Code { get; set; } = null!;
    public string? ExceptionName { get; set; }
    public string? ExceptionMessage { get; set; }
}