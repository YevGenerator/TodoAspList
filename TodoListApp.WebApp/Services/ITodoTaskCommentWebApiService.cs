using TodoListApp.Models;

namespace TodoListApp.WebApp.Services;

public interface ITodoTaskCommentWebApiService
{
    Task<IEnumerable<TodoTaskComment>> GetCommentsByTaskIdAsync(int taskId);

    Task<TodoTaskComment?> GetCommentByIdAsync(int id);

    Task CreateCommentAsync(TodoTaskComment comment);

    Task UpdateCommentAsync(TodoTaskComment comment);

    Task DeleteCommentAsync(int id);
}
