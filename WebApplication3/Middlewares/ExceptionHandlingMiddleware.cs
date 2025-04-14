using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplication3.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Передаем управление следующему middleware или конечному обработчику
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);

            Trace.WriteLine($"[TRACE] Unhandled exception: {ex.Message}");
            Trace.WriteLine($"[TRACE] StackTrace: {ex.StackTrace}");

            Console.WriteLine("!!! Ошибка !!!");
            Console.WriteLine($"Сообщение: {ex.Message}");
            Console.WriteLine($"Стек вызовов: {ex.StackTrace}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = new
            {
                type = "https://tools.ietf.org/html/rfc7807",
                title = "An unexpected error occurred.",
                status = context.Response.StatusCode,
                detail = ex.Message,
                traceId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}