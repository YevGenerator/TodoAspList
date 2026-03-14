using TodoListApp.Models;

namespace TodoListApp.WebApp.Services;

public interface ITodoTaskWebApiService
{
    Task<IEnumerable<TodoTask>> GetTasksByListIdAsync(int todoListId);

    Task<TodoTask?> GetTaskByIdAsync(int id);

    Task CreateTaskAsync(TodoTask task);

    Task UpdateTaskAsync(TodoTask task);

    Task DeleteTaskAsync(int id);
}
