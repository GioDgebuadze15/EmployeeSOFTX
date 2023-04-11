using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Employee.Services.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} received.");

        await _next(context);

        _logger.LogInformation(
            $"Response {context.Response.StatusCode} returned for {context.Request.Method} {context.Request.Path}.");
    }
}