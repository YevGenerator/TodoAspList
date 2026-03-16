using TodoListApp.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class TodoTagWebApiService : ITodoTagWebApiService
{
    private readonly HttpClient httpClient;

    public TodoTagWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoTag>> GetAllTagsAsync()
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoTagWebApiModel>>("api/todotag");
        if (response == null)
        {
            return Array.Empty<TodoTag>();
        }

        return response.Select(m => new TodoTag
        {
            Id = m.Id,
            Name = m.Name,
        });
    }

    public async Task DeleteTagAsync(int id)
    {
        _ = await this.httpClient.DeleteAsync($"api/todotag/{id}");
    }

    public async Task AssignTagToTaskAsync(int taskId, int tagId)
    {
        _ = await this.httpClient.PostAsync($"api/todotag/task/{taskId}/tag/{tagId}", null);
    }

    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        _ = await this.httpClient.DeleteAsync($"api/todotag/task/{taskId}/tag/{tagId}");
    }

    public async Task<TodoTag> CreateTagAsync(TodoTag tag)
    {
        var model = new TodoTagWebApiModel { Name = tag.Name };
        var response = await this.httpClient.PostAsJsonAsync("api/todotag", model);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<TodoTagWebApiModel>();
        tag.Id = created?.Id ?? 0;
        return tag;
    }
}
