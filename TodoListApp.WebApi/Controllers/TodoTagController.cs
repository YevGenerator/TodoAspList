using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Interfaces;
using TodoListApp.Models;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoTagController : ControllerBase
{
    private readonly ITodoTagDatabaseService tagService;

    public TodoTagController(ITodoTagDatabaseService tagService)
    {
        this.tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoTagModel>>> Get()
    {
        var tags = await this.tagService.GetAllTagsAsync();
        var models = tags.Select(t => new TodoTagModel
        {
            Id = t.Id,
            Name = t.Name,
        });

        return this.Ok(models);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTagModel>> Post([FromBody] TodoTagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var tag = new TodoTag { Name = model.Name };
        var createdTag = await this.tagService.AddTagAsync(tag);
        model.Id = createdTag.Id;

        return this.CreatedAtAction(nameof(this.Get), new { id = model.Id }, model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await this.tagService.DeleteTagAsync(id);
        return this.NoContent();
    }

    [HttpPost("task/{taskId}/tag/{tagId}")]
    public async Task<IActionResult> AssignTag(int taskId, int tagId)
    {
        await this.tagService.AssignTagToTaskAsync(taskId, tagId);
        return this.NoContent();
    }

    [HttpDelete("task/{taskId}/tag/{tagId}")]
    public async Task<IActionResult> RemoveTag(int taskId, int tagId)
    {
        await this.tagService.RemoveTagFromTaskAsync(taskId, tagId);
        return this.NoContent();
    }
}
