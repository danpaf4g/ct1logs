namespace WebApplication3.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskManager _taskManager;
    private readonly ILogger<TasksController> _logger;

    public TasksController(TaskManager taskManager, ILogger<TasksController> logger)
    {
        _taskManager = taskManager;
        _logger = logger;
    }

    // GET: api/tasks
    [HttpGet]
    public IActionResult GetTasks()
    {
        _logger.LogInformation("Listing tasks...");
        var tasks = _taskManager.ListTasks();
        return Ok(tasks);
    }

    // POST: api/tasks
    [HttpPost]
    public IActionResult AddTask([FromBody] string taskName)
    {
        _logger.LogInformation("Adding task: {TaskName}", taskName);
        _taskManager.AddTask(taskName);
        return Ok(new { message = "Task added successfully.", taskName });
    }

    // DELETE: api/tasks/{index}
    [HttpDelete("{index}")]
    public IActionResult DeleteTask(int index)
    {
        if (index >= 0 && index < _taskManager.ListTasks().Count)
        {
            _logger.LogInformation("Removing task at index: {Index}", index);
            _taskManager.RemoveTask(index);
            return Ok(new { message = "Task removed successfully.", index });
        }
        else
        {
            _logger.LogWarning("Attempted to remove invalid task index: {Index}", index);
            return BadRequest(new { message = "Invalid task index.", index });
        }
    }
}