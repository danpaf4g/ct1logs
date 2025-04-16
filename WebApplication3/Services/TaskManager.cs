using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Generic;
using WebApplication3.Models;
using WebApplication3.Models.Request;

namespace WebApplication3.Services;

public class TaskManager
{
    private readonly ILogger<TaskManager> _logger;
    private readonly List<TaskModel> _tasks = new();

    public TaskManager(ILogger<TaskManager> logger)
    {
        _logger = logger;
    }

    public TaskModel AddTask(AddTaskRequest request)
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine($"[TRACE] AddTask: Started adding task '{request.Title}'.");

        try
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException("Task title cannot be empty.");
            }

            var task = new TaskModel
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority
            };

            _tasks.Add(task);

            _logger.LogInformation("Task added: {@Task}", task);
            Trace.WriteLine($"[TRACE] AddTask: Task '{request.Title}' added successfully.");

            return task;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding task: {TaskTitle}", request.Title);
            Trace.WriteLine($"[TRACE] AddTask: Error occurred while adding task '{request.Title}'. Error: {ex.Message}");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            Trace.WriteLine($"[TRACE] AddTask: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }

    public void RemoveTask(Guid id)
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine($"[TRACE] RemoveTask: Started removing task with ID {id}.");

        try
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                _logger.LogWarning("Attempted to remove non-existent task with ID: {TaskId}", id);
                Trace.WriteLine($"[TRACE] RemoveTask: Warning - Task with ID {id} not found.");
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            _tasks.Remove(task);

            _logger.LogInformation("Task removed: {@Task}", task);
            Trace.WriteLine($"[TRACE] RemoveTask: Task '{task.Title}' with ID {id} removed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while removing task with ID: {TaskId}", id);
            Trace.WriteLine($"[TRACE] RemoveTask: Error occurred while removing task with ID {id}. Error: {ex.Message}");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            Trace.WriteLine($"[TRACE] RemoveTask: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }

    public List<TaskModel> ListTasks()
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine("[TRACE] ListTasks: Started listing tasks.");

        try
        {
            _logger.LogInformation("Listing tasks: {@Tasks}", _tasks);
            Trace.WriteLine($"[TRACE] ListTasks: Listed {_tasks.Count} tasks successfully.");

            return _tasks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while listing tasks.");
            Trace.WriteLine($"[TRACE] ListTasks: Error occurred while listing tasks. Error: {ex.Message}");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            Trace.WriteLine($"[TRACE] ListTasks: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}