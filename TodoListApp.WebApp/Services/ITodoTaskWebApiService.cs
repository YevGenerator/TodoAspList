using TodoListApp.Models;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Services;

public interface ITodoTaskWebApiService
{
    Task<IEnumerable<TodoTask>> GetTasksByListIdAsync(int todoListId);

    Task<TodoTask?> GetTaskByIdAsync(int id);

    Task UpdateTaskAsync(TodoTask task);

    Task DeleteTaskAsync(int id);

    Task<IEnumerable<TodoTask>> GetAssignedTasksAsync(string? assignee, string? tagSearch, TodoTaskStatus? status = null, string? sortBy = null);

    Task ChangeTaskStatusAsync(int id, TodoTaskStatus newStatus);

    Task<int> CreateTaskAsync(TodoTask task);
}
