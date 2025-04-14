using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskManager _taskManager;

    public TasksController(TaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    // GET: api/tasks
    [HttpGet]
    public IActionResult GetTasks()
    {
        var tasks = _taskManager.ListTasks();
        return Ok(tasks);
    }

    // POST: api/tasks
    [HttpPost]
    public IActionResult AddTask([FromBody] string taskName)
    {
        _taskManager.AddTask(taskName);
        return Ok(new { message = "Task added successfully.", taskName });
    }

    // DELETE: api/tasks/{index}
    [HttpDelete("{index}")]
    public IActionResult DeleteTask(int index)
    {
        _taskManager.RemoveTask(index);
        return Ok(new { message = "Task removed successfully.", index });
    }
}