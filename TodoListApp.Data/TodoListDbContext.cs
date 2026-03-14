using Microsoft.EntityFrameworkCore;
using TodoListApp.Data.Entities;

namespace TodoListApp.Data;

/// <summary>
/// Database context for the TodoList application.
/// </summary>
public class TodoListDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoListEntity> TodoLists { get; set; } = null!;

    public DbSet<TodoTaskEntity> TodoTasks { get; set; } = null!;

    public DbSet<TodoTagEntity> TodoTags { get; set; } = null!;

    public DbSet<TodoTaskCommentEntity> TodoTaskComments { get; set; } = null!;
}
