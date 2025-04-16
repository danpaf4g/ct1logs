namespace WebApplication3.Models.Request;

public class AddTaskRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Priority { get; set; } = "Normal";
}