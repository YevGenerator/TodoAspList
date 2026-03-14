using TodoListApp.Models;

namespace TodoListApp.Interfaces;

public interface ISearchDatabaseService
{
    Task<SearchResult> SearchAsync(string searchTerm);
}
