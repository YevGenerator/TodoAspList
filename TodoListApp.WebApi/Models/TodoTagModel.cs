using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TodoTagModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
