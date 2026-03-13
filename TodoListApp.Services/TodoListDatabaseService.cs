using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Data.Entities;
using TodoListApp.Interfaces;
using TodoListApp.Models;

namespace TodoListApp.Services;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext dbContext;

    public TodoListDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoList>> GetTodoListsAsync()
    {
        var entities = await this.dbContext.TodoLists.ToListAsync();
        return entities.Select(e => new TodoList
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
        });
    }

    public async Task<TodoList> AddTodoListAsync(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        var entity = new TodoListEntity
        {
            Title = todoList.Title,
            Description = todoList.Description,
        };

        _ = await this.dbContext.TodoLists.AddAsync(entity);
        _ = await this.dbContext.SaveChangesAsync();

        todoList!.Id = entity.Id;
        return todoList;
    }

    public async Task UpdateTodoListAsync(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        var entity = await this.dbContext.TodoLists.FindAsync(todoList.Id);
        if (entity != null)
        {
            entity.Title = todoList.Title;
            entity.Description = todoList.Description;
            _ = await this.dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteTodoListAsync(int id)
    {
        var entity = await this.dbContext.TodoLists.FindAsync(id);
        if (entity != null)
        {
            _ = this.dbContext.TodoLists.Remove(entity);
            _ = await this.dbContext.SaveChangesAsync();
        }
    }
}
