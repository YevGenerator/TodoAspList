using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Interfaces;
using TodoListApp.Models;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoTaskCommentController : ControllerBase
{
    private readonly ITodoTaskCommentDatabaseService commentService;

    public TodoTaskCommentController(ITodoTaskCommentDatabaseService commentService)
    {
        this.commentService = commentService;
    }

    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<TodoTaskCommentModel>>> GetByTaskId(int taskId)
    {
        var comments = await this.commentService.GetCommentsByTaskIdAsync(taskId);
        var models = comments.Select(c => new TodoTaskCommentModel
        {
            Id = c.Id,
            Text = c.Text,
            CreatedAt = c.CreatedAt,
            CreatedBy = c.CreatedBy,
            TodoTaskId = c.TodoTaskId,
        });

        return this.Ok(models);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTaskCommentModel>> Get(int id)
    {
        var comment = await this.commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return this.NotFound();
        }

        var model = new TodoTaskCommentModel
        {
            Id = comment.Id,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt,
            CreatedBy = comment.CreatedBy,
            TodoTaskId = comment.TodoTaskId,
        };

        return this.Ok(model);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTaskCommentModel>> Post([FromBody] TodoTaskCommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var userName = this.User.Identity?.Name ?? "Unknown";

        var comment = new TodoTaskComment
        {
            Text = model.Text,
            CreatedBy = userName,
            TodoTaskId = model.TodoTaskId,
        };

        var createdComment = await this.commentService.AddCommentAsync(comment);
        model.Id = createdComment.Id;
        model.CreatedAt = createdComment.CreatedAt;
        model.CreatedBy = createdComment.CreatedBy;

        return this.CreatedAtAction(nameof(this.Get), new { id = model.Id }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TodoTaskCommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (id != model.Id || !this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var comment = new TodoTaskComment
        {
            Id = model.Id,
            Text = model.Text,
        };

        await this.commentService.UpdateCommentAsync(comment);
        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await this.commentService.DeleteCommentAsync(id);
        return this.NoContent();
    }
}
