using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoTaskCommentController : Controller
{
    private readonly ITodoTaskCommentWebApiService commentService;

    public TodoTaskCommentController(ITodoTaskCommentWebApiService commentService)
    {
        this.commentService = commentService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoTaskCommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (this.ModelState.IsValid)
        {
            var comment = new TodoTaskComment
            {
                Text = model.Text,
                CreatedBy = "Anonymous",
                TodoTaskId = model.TodoTaskId,
            };

            await this.commentService.CreateCommentAsync(comment);
        }

        return this.RedirectToAction("Details", "TodoTask", new { id = model.TodoTaskId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var comment = await this.commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return this.NotFound();
        }

        var model = new TodoTaskCommentModel
        {
            Id = comment.Id,
            Text = comment.Text,
            TodoTaskId = comment.TodoTaskId,
        };

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TodoTaskCommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (this.ModelState.IsValid)
        {
            var comment = new TodoTaskComment
            {
                Id = model.Id,
                Text = model.Text,
                TodoTaskId = model.TodoTaskId,
            };

            await this.commentService.UpdateCommentAsync(comment);
            return this.RedirectToAction("Details", "TodoTask", new { id = model.TodoTaskId });
        }

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int taskId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        await this.commentService.DeleteCommentAsync(id);
        return this.RedirectToAction("Details", "TodoTask", new { id = taskId });
    }
}
