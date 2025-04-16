namespace WebApplication3.Models;

public class TaskModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Priority { get; set; } = "Normal";
}