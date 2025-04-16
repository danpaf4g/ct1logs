namespace WebApplication3.Models.Response;

public class AddTaskResponse
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Priority { get; set; }
}