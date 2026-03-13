using TodoListApp.Models.Enums;

namespace TodoListApp.Data.Entities;

public class TodoTaskEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public string? Assignee { get; set; }

    public int TodoListId { get; set; }

    public TodoListEntity? TodoList { get; set; }
}
