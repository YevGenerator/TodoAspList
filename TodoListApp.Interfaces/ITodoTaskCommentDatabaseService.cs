using TodoListApp.Models;

namespace TodoListApp.Interfaces;

public interface ITodoTaskCommentDatabaseService
{
    Task<IEnumerable<TodoTaskComment>> GetCommentsByTaskIdAsync(int taskId);

    Task<TodoTaskComment?> GetCommentByIdAsync(int id);

    Task<TodoTaskComment> AddCommentAsync(TodoTaskComment comment);

    Task UpdateCommentAsync(TodoTaskComment comment);

    Task DeleteCommentAsync(int id);
}
