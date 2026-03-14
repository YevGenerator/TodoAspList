namespace TodoListApp.WebApi.Models;

public class SearchResultModel
{
    public IEnumerable<TodoListModel> TodoLists { get; set; } = new List<TodoListModel>();

    public IEnumerable<TodoTaskModel> TodoTasks { get; set; } = new List<TodoTaskModel>();
}
