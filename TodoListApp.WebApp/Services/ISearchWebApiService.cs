using TodoListApp.Models;

namespace TodoListApp.WebApp.Services;

public interface ISearchWebApiService
{
    Task<SearchResult> SearchAsync(string query);
}
