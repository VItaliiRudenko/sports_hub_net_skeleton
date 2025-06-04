using System.Net;
using System.Text.Json;

namespace SportsHub.Api.Middlewares;

/// <summary>
/// Global exception handling middleware for consistent error responses
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid request parameters";
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Unauthorized access";
                break;
            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = "Resource not found";
                break;
            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid operation";
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An error occurred while processing your request";
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = new
            {
                message,
                details = _environment.IsDevelopment() ? exception.Message : message,
                traceId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            }
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        await context.Response.WriteAsync(jsonResponse);
    }
} 