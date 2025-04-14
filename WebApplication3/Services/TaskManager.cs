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

        var taskInfo = new { TaskName = taskName, Timestamp = DateTime.UtcNow };
        _tasks.Add(taskName);

        _logger.LogInformation("Task added: {@TaskInfo}", taskInfo);
        Trace.WriteLine($"[TRACE] AddTask: Task '{taskName}' added successfully.");

        stopwatch.Stop();
        Trace.WriteLine($"[TRACE] AddTask: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");
    }

    public void RemoveTask(int index)
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine($"[TRACE] RemoveTask: Started removing task at index {index}.");

        if (index < 0 || index >= _tasks.Count)
        {
            var errorInfo = new { AttemptedIndex = index, Timestamp = DateTime.UtcNow };
            _logger.LogError("Attempted to remove invalid task index: {@ErrorInfo}", errorInfo);
            Trace.WriteLine($"[TRACE] RemoveTask: Error - Invalid task index {index}.");
            throw new ArgumentOutOfRangeException(nameof(index), $"Invalid task index: {index}");
        }

        var removedTask = _tasks[index];
        var taskInfo = new { TaskName = removedTask, Index = index, Timestamp = DateTime.UtcNow };
        _tasks.RemoveAt(index);

        _logger.LogInformation("Task removed: {@TaskInfo}", taskInfo);
        Trace.WriteLine($"[TRACE] RemoveTask: Task '{removedTask}' at index {index} removed successfully.");

        stopwatch.Stop();
        Trace.WriteLine($"[TRACE] RemoveTask: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");
    }

    public List<string> ListTasks()
    {
        var stopwatch = Stopwatch.StartNew();
        Trace.WriteLine("[TRACE] ListTasks: Started listing tasks.");

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

        stopwatch.Stop();
        Trace.WriteLine($"[TRACE] ListTasks: Operation completed in {stopwatch.ElapsedMilliseconds} ms.");

        return _tasks;
    }
}