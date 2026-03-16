namespace TodoListApp.Models;

public class TodoList
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string OwnerId { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<TodoTask> Tasks { get; init; } = new List<TodoTask>();
}
