namespace TodoListApp.Models;

public class SearchResult
{
    public IEnumerable<TodoList> TodoLists { get; set; } = new List<TodoList>();

    public IEnumerable<TodoTask> TodoTasks { get; set; } = new List<TodoTask>();
}
