using TodoListApp.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient httpClient;

    public TodoListWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoList>> GetTodoListsAsync()
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoListWebApiModel>>("api/todolist");
        if (response == null)
        {
            return Array.Empty<TodoList>();
        }

        return response.Select(m => new TodoList
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            Tasks = m.Tasks?.Select(t => new TodoTask
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                DueDate = t.DueDate,
                TodoListId = t.TodoListId,
                Tags = t.Tags?.Select(tg => new TodoTag { Id = tg.Id, Name = tg.Name }).ToList() ?? new List<TodoTag>(),
            }).ToList() ?? new List<TodoTask>(),
        });
    }

    public async Task<TodoList?> GetTodoListByIdAsync(int id)
    {
        var lists = await this.GetTodoListsAsync();
        return lists.FirstOrDefault(l => l.Id == id);
    }

    public async Task CreateTodoListAsync(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var model = new TodoListWebApiModel
        {
            Title = todoList.Title,
            Description = todoList.Description,
        };

        _ = await this.httpClient.PostAsJsonAsync("api/todolist", model);
    }

    public async Task UpdateTodoListAsync(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var model = new TodoListWebApiModel
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Description = todoList.Description,
        };

        _ = await this.httpClient.PutAsJsonAsync($"api/todolist/{todoList.Id}", model);
    }

    public async Task DeleteTodoListAsync(int id)
    {
        _ = await this.httpClient.DeleteAsync(new Uri($"api/todolist/{id}", UriKind.Relative));
    }
}
