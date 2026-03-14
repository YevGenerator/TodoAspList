namespace TodoListApp.WebApp.Models;

public class SearchResultModel
{
    public string SearchQuery { get; set; } = string.Empty;

    public IEnumerable<TodoListModel> TodoLists { get; set; } = new List<TodoListModel>();

    public IEnumerable<TodoTaskModel> TodoTasks { get; set; } = new List<TodoTaskModel>();
}
