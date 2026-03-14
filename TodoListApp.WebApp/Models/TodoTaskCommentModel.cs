using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class TodoTaskCommentModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Comment text is required")]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public int TodoTaskId { get; set; }
}
