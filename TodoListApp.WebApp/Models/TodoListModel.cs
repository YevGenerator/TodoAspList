using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class TodoListModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}
