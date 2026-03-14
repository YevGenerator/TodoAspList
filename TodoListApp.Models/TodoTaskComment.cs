namespace TodoListApp.Models;

public class TodoTaskComment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public int TodoTaskId { get; set; }
}
