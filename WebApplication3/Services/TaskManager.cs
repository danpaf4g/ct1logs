using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WebApplication3.Services;

public class TaskManager
{
    private readonly ILogger<TaskManager> _logger;
    private readonly List<string> _tasks = new();

    public TaskManager(ILogger<TaskManager> logger)
    {
        _logger = logger;
    }

    public void AddTask(string taskName)
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine($"[TRACE] AddTask: Started adding task '{taskName}'.");

        try
        {
            var taskInfo = new { TaskName = taskName, Timestamp = DateTime.UtcNow };
            _tasks.Add(taskName);

            _logger.LogInformation("Task added: {@TaskInfo}", taskInfo);
            Trace.WriteLine($"[TRACE] AddTask: Task '{taskName}' added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding task: {TaskName}", taskName);
            Trace.WriteLine($"[TRACE] AddTask: Error occurred while adding task '{taskName}'. Error: {ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
            Trace.WriteLine($"[TRACE] AddTask: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }

    public void RemoveTask(int index)
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine($"[TRACE] RemoveTask: Started removing task at index {index}.");

        try
        {
            if (index >= 0 && index < _tasks.Count)
            {
                var removedTask = _tasks[index];
                var taskInfo = new { TaskName = removedTask, Index = index, Timestamp = DateTime.UtcNow };
                _tasks.RemoveAt(index);

                _logger.LogInformation("Task removed: {@TaskInfo}", taskInfo);
                Trace.WriteLine($"[TRACE] RemoveTask: Task '{removedTask}' at index {index} removed successfully.");
            }
            else
            {
                var errorInfo = new { AttemptedIndex = index, Timestamp = DateTime.UtcNow };
                _logger.LogWarning("Attempted to remove invalid task index: {@ErrorInfo}", errorInfo);
                Trace.WriteLine($"[TRACE] RemoveTask: Warning - Invalid task index {index}.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while removing task at index: {Index}", index);
            Trace.WriteLine($"[TRACE] RemoveTask: Error occurred while removing task at index {index}. Error: {ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
            Trace.WriteLine($"[TRACE] RemoveTask: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }

    public List<string> ListTasks()
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine("[TRACE] ListTasks: Started listing tasks.");

        try
        {
            var tasksInfo = new { Tasks = _tasks, Count = _tasks.Count, Timestamp = DateTime.UtcNow };

            if (_tasks.Count == 0)
            {
                _logger.LogInformation("No tasks available: {@TasksInfo}", tasksInfo);
                Trace.WriteLine("[TRACE] ListTasks: No tasks available.");
            }
            else
            {
                _logger.LogInformation("Listing tasks: {@TasksInfo}", tasksInfo);
                Trace.WriteLine($"[TRACE] ListTasks: Listed {tasksInfo.Count} tasks successfully.");
            }

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