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
        var taskInfo = new { TaskName = taskName, Timestamp = DateTime.UtcNow };
        _tasks.Add(taskName);
        _logger.LogInformation("Task added: {@TaskInfo}", taskInfo);
        Trace.WriteLine($"Task added: {taskName}");
    }

    public void RemoveTask(int index)
    {
        if (index >= 0 && index < _tasks.Count)
        {
            var removedTask = _tasks[index];
            var taskInfo = new { TaskName = removedTask, Index = index, Timestamp = DateTime.UtcNow };
            _tasks.RemoveAt(index);
            _logger.LogInformation("Task removed: {@TaskInfo}", taskInfo); // Логируем объект
            Trace.WriteLine($"Task removed: {removedTask} at index: {index}");
        }
        else
        {
            var errorInfo = new { AttemptedIndex = index, Timestamp = DateTime.UtcNow };
            _logger.LogWarning("Attempted to remove invalid task index: {@ErrorInfo}", errorInfo);
            Trace.WriteLine($"Attempted to remove invalid task index: {index}");
        }
    }

    public List<string> ListTasks()
    {
        var tasksInfo = new { Tasks = _tasks, Count = _tasks.Count, Timestamp = DateTime.UtcNow };

        if (_tasks.Count == 0)
        {
            _logger.LogInformation("No tasks available: {@TasksInfo}", tasksInfo);
            Trace.WriteLine("No tasks available.");
        }
        else
        {
            _logger.LogInformation("Listing tasks: {@TasksInfo}", tasksInfo);
            Trace.WriteLine("Listing tasks...");
        }
        return _tasks;
    }
}