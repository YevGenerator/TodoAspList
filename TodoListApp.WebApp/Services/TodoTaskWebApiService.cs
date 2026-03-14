using TodoListApp.Models;
using TodoListApp.Models.Enums;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class TodoTaskWebApiService : ITodoTaskWebApiService
{
    private readonly HttpClient httpClient;

    public TodoTaskWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoTask>> GetTasksByListIdAsync(int todoListId)
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoTaskWebApiModel>>($"api/todotask/list/{todoListId}");
        if (response == null)
        {
            return Array.Empty<TodoTask>();
        }

        return response.Select(m => new TodoTask
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            DueDate = m.DueDate,
            Status = m.Status,
            Assignee = m.Assignee,
            TodoListId = m.TodoListId,
        });
    }

    public async Task<TodoTask?> GetTaskByIdAsync(int id)
    {
        var response = await this.httpClient.GetFromJsonAsync<TodoTaskWebApiModel>($"api/todotask/{id}");
        if (response == null)
        {
            return null;
        }

        return new TodoTask
        {
            Id = response.Id,
            Title = response.Title,
            Description = response.Description,
            DueDate = response.DueDate,
            Status = response.Status,
            Assignee = response.Assignee,
            TodoListId = response.TodoListId,
        };
    }

    public async Task CreateTaskAsync(TodoTask task)
    {
        ArgumentNullException.ThrowIfNull(task);

        var model = new TodoTaskWebApiModel
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            TodoListId = task.TodoListId,
        };

        _ = await this.httpClient.PostAsJsonAsync("api/todotask", model);
    }

    public async Task UpdateTaskAsync(TodoTask task)
    {
        ArgumentNullException.ThrowIfNull(task);

        var model = new TodoTaskWebApiModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            TodoListId = task.TodoListId,
        };

        _ = await this.httpClient.PutAsJsonAsync($"api/todotask/{task.Id}", model);
    }

    public async Task DeleteTaskAsync(int id)
    {
        _ = await this.httpClient.DeleteAsync(new Uri($"api/todotask/{id}", UriKind.Relative));
    }

    public async Task<IEnumerable<TodoTask>> GetAssignedTasksAsync(string assignee, TodoTaskStatus? status = null, string? sortBy = null)
    {
        var url = $"api/todotask/assigned?assignee={Uri.EscapeDataString(assignee)}";
        if (status.HasValue)
        {
            url += $"&status={status.Value}";
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            url += $"&sortBy={Uri.EscapeDataString(sortBy)}";
        }

        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoTaskWebApiModel>>(url);
        if (response == null)
        {
            return Array.Empty<TodoTask>();
        }

        return response.Select(m => new TodoTask
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            DueDate = m.DueDate,
            Status = m.Status,
            Assignee = m.Assignee,
            TodoListId = m.TodoListId,
        });
    }

    public async Task ChangeTaskStatusAsync(int id, TodoTaskStatus newStatus)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/todotask/{id}/status", newStatus);
        _ = response.EnsureSuccessStatusCode();
    }
}
