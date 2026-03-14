using System.Globalization;
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

        var term = searchTerm.ToLower(CultureInfo.CurrentCulture);

        var lists = await this.dbContext.TodoLists
            .Where(l => l.Title.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                       (l.Description != null && l.Description.Contains(term, StringComparison.CurrentCultureIgnoreCase)))
            .ToListAsync();

        var tasks = await this.dbContext.TodoTasks
            .Where(t => t.Title.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                       (t.Description != null && t.Description.Contains(term, StringComparison.CurrentCultureIgnoreCase)))
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
