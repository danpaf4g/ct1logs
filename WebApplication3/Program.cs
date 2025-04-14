using System.Diagnostics;
using Serilog;
using WebApplication3.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: Serilog.RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

Trace.Listeners.Add(new TextWriterTraceListener(File.CreateText("trace.log")));
Trace.AutoFlush = true;

builder.Services.AddSingleton<TaskManager>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "API for managing tasks with logging and tracing."
    });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapGet("/test", (ILogger<Program> logger) =>
{
    logger.LogInformation("test");
    Log.Information("test(Serilog).");
    Trace.WriteLine("test(Trace).");
    return "test";
});

app.MapGet("/tasks", (TaskManager taskManager) =>
{
    var result = taskManager.ListTasks();
    return result;
})
.WithName("GetTasks")
.WithOpenApi();

app.MapPost("/tasks", (TaskManager taskManager, string taskName) =>
{
    taskManager.AddTask(taskName);
    return Results.Ok(new { message = "Task added successfully." });
})
.WithName("AddTask")
.WithOpenApi();

app.MapDelete("/tasks/{index}", (TaskManager taskManager, int index) =>
{
    taskManager.RemoveTask(index);
    return Results.Ok(new { message = "Task removed successfully." });
})
.WithName("RemoveTask")
.WithOpenApi();

app.Run();