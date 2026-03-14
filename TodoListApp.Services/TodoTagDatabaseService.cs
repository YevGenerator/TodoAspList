using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Data.Entities;
using TodoListApp.Interfaces;
using TodoListApp.Models;

namespace TodoListApp.Services;

public class TodoTagDatabaseService : ITodoTagDatabaseService
{
    private readonly TodoListDbContext dbContext;

    public TodoTagDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<TodoTag>> GetAllTagsAsync()
    {
        var entities = await this.dbContext.TodoTags.ToListAsync();
        return entities.Select(e => new TodoTag
        {
            Id = e.Id,
            Name = e.Name,
        });
    }

    public async Task<TodoTag> AddTagAsync(TodoTag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        var entity = new TodoTagEntity
        {
            Name = tag.Name,
        };

        _ = await this.dbContext.TodoTags.AddAsync(entity);
        _ = await this.dbContext.SaveChangesAsync();

        tag.Id = entity.Id;
        return tag;
    }

    public async Task DeleteTagAsync(int id)
    {
        var entity = await this.dbContext.TodoTags.FindAsync(id);
        if (entity != null)
        {
            _ = this.dbContext.TodoTags.Remove(entity);
            _ = await this.dbContext.SaveChangesAsync();
        }
    }

    public async Task AssignTagToTaskAsync(int taskId, int tagId)
    {
        var task = await this.dbContext.TodoTasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
        var tag = await this.dbContext.TodoTags.FindAsync(tagId);

        if (task != null && tag != null && !task.Tags.Any(t => t.Id == tagId))
        {
            task.Tags.Add(tag);
            _ = await this.dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var task = await this.dbContext.TodoTasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
        var tag = task?.Tags.FirstOrDefault(t => t.Id == tagId);

        if (task != null && tag != null)
        {
            _ = task.Tags.Remove(tag);
            _ = await this.dbContext.SaveChangesAsync();
        }
    }
}
