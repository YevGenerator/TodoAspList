namespace TodoListApp.WebApp.Models;

public class TodoListWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<TodoTaskWebApiModel> Tasks { get; init; } = new List<TodoTaskWebApiModel>();
}
