using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApplication3.Middlewares;

public class TracingMiddleware
{
    private readonly RequestDelegate _next;

    public TracingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine($"[TRACE] Request started: {context.Request.Method} {context.Request.Path}");

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            Trace.WriteLine($"[TRACE] Request completed: {context.Request.Method} {context.Request.Path} in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}