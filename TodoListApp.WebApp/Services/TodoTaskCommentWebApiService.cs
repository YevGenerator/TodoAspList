using TodoListApp.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class TodoTaskCommentWebApiService : ITodoTaskCommentWebApiService
{
    private readonly HttpClient httpClient;

    public TodoTaskCommentWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoTaskComment>> GetCommentsByTaskIdAsync(int taskId)
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoTaskCommentWebApiModel>>($"api/todotaskcomment/task/{taskId}");
        if (response == null)
        {
            return Array.Empty<TodoTaskComment>();
        }

        return response.Select(c => new TodoTaskComment
        {
            Id = c.Id,
            Text = c.Text,
            CreatedAt = c.CreatedAt,
            CreatedBy = c.CreatedBy,
            TodoTaskId = c.TodoTaskId,
        });
    }

    public async Task<TodoTaskComment?> GetCommentByIdAsync(int id)
    {
        var response = await this.httpClient.GetFromJsonAsync<TodoTaskCommentWebApiModel>($"api/todotaskcomment/{id}");
        if (response == null)
        {
            return null;
        }

        return new TodoTaskComment
        {
            Id = response.Id,
            Text = response.Text,
            CreatedAt = response.CreatedAt,
            CreatedBy = response.CreatedBy,
            TodoTaskId = response.TodoTaskId,
        };
    }

    public async Task CreateCommentAsync(TodoTaskComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        var model = new TodoTaskCommentWebApiModel
        {
            Text = comment.Text,
            CreatedBy = comment.CreatedBy,
            TodoTaskId = comment.TodoTaskId,
        };
        var response = await this.httpClient.PostAsJsonAsync("api/todotaskcomment", model);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCommentAsync(TodoTaskComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var model = new TodoTaskCommentWebApiModel
        {
            Id = comment.Id,
            Text = comment.Text,
            TodoTaskId = comment.TodoTaskId,
        };
        var response = await this.httpClient.PutAsJsonAsync($"api/todotaskcomment/{comment.Id}", model);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"api/todotaskcomment/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
