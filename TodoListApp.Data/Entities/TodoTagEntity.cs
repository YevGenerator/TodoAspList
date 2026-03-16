namespace TodoListApp.Data.Entities;

public class TodoTagEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<TodoTaskEntity> Tasks { get; init; } = new List<TodoTaskEntity>();
}
