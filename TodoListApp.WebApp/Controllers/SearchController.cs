using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class SearchController : Controller
{
    private readonly ISearchWebApiService searchService;

    public SearchController(ISearchWebApiService searchService)
    {
        this.searchService = searchService;
    }

    public async Task<IActionResult> Index(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return this.View(new SearchResultModel { SearchQuery = string.Empty });
        }

        var result = await this.searchService.SearchAsync(query);

        var model = new SearchResultModel
        {
            SearchQuery = query,
            TodoLists = result.TodoLists.Select(l => new TodoListModel
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
            }),
            TodoTasks = result.TodoTasks.Select(t => new TodoTaskModel
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

        return this.View(model);
    }
}
