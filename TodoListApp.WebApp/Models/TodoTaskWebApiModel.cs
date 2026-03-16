using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Models;

public class TodoTaskWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public string? Assignee { get; set; }

    public int TodoListId { get; set; }

    public ICollection<TodoTagWebApiModel> Tags { get; init; } = new List<TodoTagWebApiModel>();
}
