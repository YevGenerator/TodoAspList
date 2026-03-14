namespace TodoListApp.WebApp.Models;

public class SearchResultWebApiModel
{
    public IEnumerable<TodoListWebApiModel> TodoLists { get; set; } = new List<TodoListWebApiModel>();

    public IEnumerable<TodoTaskWebApiModel> TodoTasks { get; set; } = new List<TodoTaskWebApiModel>();
}
