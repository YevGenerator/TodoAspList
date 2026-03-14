using TodoListApp.Models;

namespace TodoListApp.Interfaces;

public interface ITodoTagDatabaseService
{
    Task<IEnumerable<TodoTag>> GetAllTagsAsync();

    Task<TodoTag> AddTagAsync(TodoTag tag);

    Task DeleteTagAsync(int id);

    Task AssignTagToTaskAsync(int taskId, int tagId);

    Task RemoveTagFromTaskAsync(int taskId, int tagId);
}
