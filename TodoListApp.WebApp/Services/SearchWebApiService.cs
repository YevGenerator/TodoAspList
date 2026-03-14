using TodoListApp.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class SearchWebApiService : ISearchWebApiService
{
    private readonly HttpClient httpClient;

    public SearchWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<SearchResult> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new SearchResult();
        }

        var response = await this.httpClient.GetFromJsonAsync<SearchResultWebApiModel>($"api/search?query={Uri.EscapeDataString(query)}");
        if (response == null)
        {
            return new SearchResult();
        }

        return new SearchResult
        {
            TodoLists = response.TodoLists.Select(l => new TodoList
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
            }),
            TodoTasks = response.TodoTasks.Select(t => new TodoTask
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                Assignee = t.Assignee,
                TodoListId = t.TodoListId,
            }),
        };
    }
}
