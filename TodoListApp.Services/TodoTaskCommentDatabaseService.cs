using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Data.Entities;
using TodoListApp.Interfaces;
using TodoListApp.Models;

namespace TodoListApp.Services;

public class TodoTaskCommentDatabaseService : ITodoTaskCommentDatabaseService
{
    private readonly TodoListDbContext dbContext;

    public TodoTaskCommentDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoTaskComment>> GetCommentsByTaskIdAsync(int taskId)
    {
        var entities = await this.dbContext.TodoTaskComments
            .Where(c => c.TodoTaskId == taskId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return entities.Select(e => new TodoTaskComment
        {
            Id = e.Id,
            Text = e.Text,
            CreatedAt = e.CreatedAt,
            CreatedBy = e.CreatedBy,
            TodoTaskId = e.TodoTaskId,
        });
    }

    public async Task<TodoTaskComment?> GetCommentByIdAsync(int id)
    {
        var entity = await this.dbContext.TodoTaskComments.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        return new TodoTaskComment
        {
            Id = entity.Id,
            Text = entity.Text,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            TodoTaskId = entity.TodoTaskId,
        };
    }

    public async Task<TodoTaskComment> AddCommentAsync(TodoTaskComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var entity = new TodoTaskCommentEntity
        {
            Text = comment.Text,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = comment.CreatedBy,
            TodoTaskId = comment.TodoTaskId,
        };

        _ = await this.dbContext.TodoTaskComments.AddAsync(entity);
        _ = await this.dbContext.SaveChangesAsync();

        comment.Id = entity.Id;
        comment.CreatedAt = entity.CreatedAt;
        return comment;
    }

    public async Task UpdateCommentAsync(TodoTaskComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var entity = await this.dbContext.TodoTaskComments.FindAsync(comment.Id);
        if (entity != null)
        {
            entity.Text = comment.Text;
            _ = await this.dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteCommentAsync(int id)
    {
        var entity = await this.dbContext.TodoTaskComments.FindAsync(id);
        if (entity != null)
        {
            _ = this.dbContext.TodoTaskComments.Remove(entity);
            _ = await this.dbContext.SaveChangesAsync();
        }
    }
}
