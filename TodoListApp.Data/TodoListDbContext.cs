using Microsoft.EntityFrameworkCore;

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
}
