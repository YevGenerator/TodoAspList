using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TodoTaskCommentModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    [Required]
    public int TodoTaskId { get; set; }
}
