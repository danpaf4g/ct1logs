using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplication3.Middlewares;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception)
        {
            throw;
        }

        if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest && context.Request.Path.StartsWithSegments("/api"))
        {
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Bad request. Please check your input.",
                Errors = context.Items["Errors"] ?? new { }
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}