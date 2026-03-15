using TodoListApp.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public interface ITodoTagWebApiService
{
    Task<IEnumerable<TodoTag>> GetAllTagsAsync();

    Task DeleteTagAsync(int id);

    Task AssignTagToTaskAsync(int taskId, int tagId);

    Task RemoveTagFromTaskAsync(int taskId, int tagId);

    Task<TodoTag> CreateTagAsync(TodoTag tag);
}
