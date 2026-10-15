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
            var statusCode = GetStatusCode(ex);
            
            var logLevel = statusCode >= 500
                ? LogLevel.Error
                : LogLevel.Warning;
            
            _logger.Log(
                logLevel,
                ex,
                "Request failed with status {StatusCode}. Exception {ExceptionType} on {RequestMethod} {RequestPath}",
                statusCode,
                ex.GetType().Name,
                context.Request.Method,
                context.Request.Path);
            
            await HandleException(context, ex, statusCode);
        }
    }
    
    private static int GetStatusCode(Exception ex)
    {
        return ex switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            ConflictException => StatusCodes.Status409Conflict,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
    
    private static async Task HandleException(
        HttpContext context, 
        Exception ex,
        int statusCode)
    {
        var response = new ApiErrorResponse
        {
            Code = ex.GetType().Name,
            ExceptionName = ex.GetType().Name,
            ExceptionMessage = ex.Message
        };

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(response);
    }
}