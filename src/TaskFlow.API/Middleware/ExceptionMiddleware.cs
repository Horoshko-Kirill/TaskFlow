using FluentValidation;
using TaskFlow.Application.DTO.Common;
using TaskFlow.Application.Exceptions;

namespace TaskFlow.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Api error {message}", ex.Message);
            await HandleException(context, ex);
        }
    }
    
    private static async Task HandleException(HttpContext context, Exception ex)
    {
        var response = new ApiErrorResponse
        {
            Code = ex.GetType().Name,
            ExceptionName = ex.GetType().Name,
            ExceptionMessage = ex.Message,
            StackTrace = ex.StackTrace?.Split('\n').ToList()
        };

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = ex switch
        {
            BadRequestException => 400,
            ValidationException => 400,
            ConflictException => 409,
            NotFoundException => 404,
            _ => 500
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}