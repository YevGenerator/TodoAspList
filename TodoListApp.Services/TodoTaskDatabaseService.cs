using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Data.Entities;
using TodoListApp.Interfaces;
using TodoListApp.Models;

namespace TodoListApp.Services;

public class TodoTaskDatabaseService : ITodoTaskDatabaseService
{
    private readonly TodoListDbContext dbContext;

    public TodoTaskDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoTask>> GetTasksByListIdAsync(int todoListId)
    {
        var entities = await this.dbContext.TodoTasks
            .Where(t => t.TodoListId == todoListId)
            .ToListAsync();

        return entities.Select(e => new TodoTask
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            CreatedDate = e.CreatedDate,
            DueDate = e.DueDate,
            Status = e.Status,
            Assignee = e.Assignee,
            TodoListId = e.TodoListId,
        });
    }

    public async Task<TodoTask?> GetTaskByIdAsync(int id)
    {
        var entity = await this.dbContext.TodoTasks.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        return new TodoTask
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CreatedDate = entity.CreatedDate,
            DueDate = entity.DueDate,
            Status = entity.Status,
            Assignee = entity.Assignee,
            TodoListId = entity.TodoListId,
        };
    }

    public async Task<TodoTask> AddTaskAsync(TodoTask task)
    {
        ArgumentNullException.ThrowIfNull(task);

        var entity = new TodoTaskEntity
        {
            Title = task.Title,
            Description = task.Description,
            CreatedDate = DateTime.UtcNow,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            TodoListId = task.TodoListId,
        };

        _ = await this.dbContext.TodoTasks.AddAsync(entity);
        _ = await this.dbContext.SaveChangesAsync();

        task.Id = entity.Id;
        task.CreatedDate = entity.CreatedDate;
        return task;
    }

    public async Task UpdateTaskAsync(TodoTask task)
    {
        ArgumentNullException.ThrowIfNull(task);

        var entity = await this.dbContext.TodoTasks.FindAsync(task.Id);
        if (entity != null)
        {
            entity.Title = task.Title;
            entity.Description = task.Description;
            entity.DueDate = task.DueDate;
            entity.Status = task.Status;
            entity.Assignee = task.Assignee;

            _ = await this.dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteTaskAsync(int id)
    {
        var entity = await this.dbContext.TodoTasks.FindAsync(id);
        if (entity != null)
        {
            _ = this.dbContext.TodoTasks.Remove(entity);
            _ = await this.dbContext.SaveChangesAsync();
        }
    }
}
