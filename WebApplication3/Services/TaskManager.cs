using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WebApplication3.Services;

public class TaskManager
{
    private readonly ILogger _logger;
    private readonly List<string> _tasks = new();

    public TaskManager(ILogger<TaskManager> logger)
    {
        _logger = logger;
    }

    public void AddTask(string taskName)
    {
        _tasks.Add(taskName);
        _logger.LogInformation($"Task added: {taskName}");
        Trace.WriteLine($"Task added: {taskName}");
    }

    public void RemoveTask(int index)
    {
        if (index >= 0 && index < _tasks.Count)
        {
            var removedTask = _tasks[index];
            _tasks.RemoveAt(index);
            _logger.LogInformation($"Task removed: {removedTask}");
            Trace.WriteLine($"Task removed: {removedTask}");
        }
        else
        {
            _logger.LogWarning($"Attempted to remove invalid task index: {index}");
            Trace.WriteLine($"Attempted to remove invalid task index: {index}");
        }
    }

    public List<string> ListTasks()
    {
        if (_tasks.Count == 0)
        {
            _logger.LogInformation("No tasks available.");
            Trace.WriteLine("No tasks available.");
        }
        else
        {
            _logger.LogInformation("Listing tasks...");
            Trace.WriteLine("Listing tasks...");
        }
        return _tasks;
    }
}