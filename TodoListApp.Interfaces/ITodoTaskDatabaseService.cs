using TodoListApp.Models;

namespace TodoListApp.Interfaces;

public interface ITodoTaskDatabaseService
{
    Task<IEnumerable<TodoTask>> GetTasksByListIdAsync(int todoListId);

    Task<TodoTask?> GetTaskByIdAsync(int id);

    Task<TodoTask> AddTaskAsync(TodoTask task);

    Task UpdateTaskAsync(TodoTask task);

    Task DeleteTaskAsync(int id);
}
