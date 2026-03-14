using TodoListApp.Models.Enums;

namespace TodoListApp.Models;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public string? Assignee { get; set; }

    public int TodoListId { get; set; }

    public List<TodoTag> Tags { get; set; } = new List<TodoTag>();
}
