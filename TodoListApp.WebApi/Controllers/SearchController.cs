using Microsoft.AspNetCore.Mvc;
using TodoListApp.Interfaces;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchDatabaseService searchService;

    public SearchController(ISearchDatabaseService searchService)
    {
        this.searchService = searchService;
    }

    [HttpGet]
    public async Task<ActionResult<SearchResultModel>> Get([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return this.BadRequest("Search query cannot be empty.");
        }

        var result = await this.searchService.SearchAsync(query);

        var model = new SearchResultModel
        {
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

        return this.Ok(model);
    }
}
