using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services;
using WebApplication3.Models;
using WebApplication3.Models.Request;
using WebApplication3.Models.Response;

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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetTasks()
    {
        var tasks = _taskManager.ListTasks();
        return Ok(tasks);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddTask([FromBody] AddTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { message = "Task title cannot be empty." });
        }

        var task = _taskManager.AddTask(request);

        // Формируем ответ
        var response = new AddTaskResponse
        {
            TaskId = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority
        };

        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteTask(Guid id)
    {
        try
        {
            _taskManager.RemoveTask(id);
            return Ok(new { message = "Task removed successfully.", id });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}