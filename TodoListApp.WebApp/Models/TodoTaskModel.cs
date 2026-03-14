using System.ComponentModel.DataAnnotations;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Models;

public class TodoTaskModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public string? Assignee { get; set; }

    public int TodoListId { get; set; }

    public List<TodoTagModel> Tags { get; set; } = new List<TodoTagModel>();

    public List<TodoTaskCommentModel> Comments { get; set; } = new List<TodoTaskCommentModel>();
}
