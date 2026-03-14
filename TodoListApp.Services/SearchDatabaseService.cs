using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Interfaces;
using TodoListApp.Models;

namespace TodoListApp.Services;

public class SearchDatabaseService : ISearchDatabaseService
{
    private readonly TodoListDbContext dbContext;

    public SearchDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<SearchResult> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new SearchResult();
        }

        var pattern = $"%{searchTerm}%";

        var lists = await this.dbContext.TodoLists
            .Where(l => EF.Functions.Like(l.Title, pattern) ||
                       (l.Description != null && EF.Functions.Like(l.Description, pattern)))
            .ToListAsync();

        var tasks = await this.dbContext.TodoTasks
            .Where(t => EF.Functions.Like(t.Title, pattern) ||
                       (t.Description != null && EF.Functions.Like(t.Description, pattern)))
            .ToListAsync();

        var searchResult = new SearchResult
        {
            TodoLists = lists.Select(e => new TodoList
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
            }),
            TodoTasks = tasks.Select(e => new TodoTask
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                DueDate = e.DueDate,
                Status = e.Status,
                Assignee = e.Assignee,
                TodoListId = e.TodoListId,
            }),
        };

        return searchResult;
    }
}
