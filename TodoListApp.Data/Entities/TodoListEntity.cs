namespace TodoListApp.Data.Entities;

public class TodoListEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string OwnerId { get; set; } = string.Empty;

    public ICollection<TodoTaskEntity> Tasks { get; set; } = new List<TodoTaskEntity>();
}
