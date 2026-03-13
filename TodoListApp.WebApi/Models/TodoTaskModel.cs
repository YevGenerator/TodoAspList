using System.ComponentModel.DataAnnotations;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Models;

public class TodoTaskModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public string? Assignee { get; set; }

    [Required]
    public int TodoListId { get; set; }
}
