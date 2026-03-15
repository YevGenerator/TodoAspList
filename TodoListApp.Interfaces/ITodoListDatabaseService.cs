using TodoListApp.Models;

namespace TodoListApp.Interfaces;

public interface ITodoListDatabaseService
{
    Task<IEnumerable<TodoList>> GetTodoListsAsync(string userId);

    Task<TodoList> AddTodoListAsync(TodoList todoList);

    Task UpdateTodoListAsync(TodoList todoList);

    Task DeleteTodoListAsync(int id);
}
