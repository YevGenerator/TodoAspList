using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using TodoListApp.Models.Enums;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoTaskController : Controller
{
    private readonly ITodoTaskWebApiService taskService;
    private readonly ITodoTagWebApiService tagService;
    private readonly ITodoTaskCommentWebApiService commentService;

    public TodoTaskController(
        ITodoTaskWebApiService taskService,
        ITodoTagWebApiService tagService,
        ITodoTaskCommentWebApiService commentService)
    {
        this.taskService = taskService;
        this.tagService = tagService;
        this.commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        this.ViewBag.TodoListId = todoListId;
        var tasks = await this.taskService.GetTasksByListIdAsync(todoListId);

        var models = tasks.Select(t => new TodoTaskModel
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            Status = t.Status,
            Assignee = t.Assignee,
            TodoListId = t.TodoListId,
        });

        return this.View(models);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var task = await this.taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        var comments = await this.commentService.GetCommentsByTaskIdAsync(id);

        var model = new TodoTaskModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            TodoListId = task.TodoListId,
            Tags = task.Tags.Select(t => new TodoTagModel { Id = t.Id, Name = t.Name }).ToList(),
            Comments = comments.Select(c => new TodoTaskCommentModel { Id = c.Id, Text = c.Text, CreatedAt = c.CreatedAt, CreatedBy = c.CreatedBy, TodoTaskId = c.TodoTaskId }).ToList(),
        };

        var allTags = await this.tagService.GetAllTagsAsync();
        this.ViewBag.AllTags = allTags.Select(t => t.Name).ToList();

        return this.View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var model = new TodoTaskModel { TodoListId = todoListId };
        var allTags = await this.tagService.GetAllTagsAsync();
        this.ViewBag.AllTags = allTags.Select(t => t.Name).ToList();
        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (this.ModelState.IsValid)
        {
            var task = new TodoTask
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                Status = model.Status,
                Assignee = model.Assignee,
                TodoListId = model.TodoListId,
            };

            int newTaskId = await this.taskService.CreateTaskAsync(task);
            await this.ProcessTagsOnTheFlyAsync(newTaskId, model.TagNames);
            return this.RedirectToAction(nameof(this.Index), new { todoListId = model.TodoListId });
        }

        return this.View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var task = await this.taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        var model = new TodoTaskModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            TodoListId = task.TodoListId,
            TagNames = string.Join(" ", task.Tags.Select(t => t.Name)),
        };

        var allTags = await this.tagService.GetAllTagsAsync();
        this.ViewBag.AllTags = allTags.Select(t => t.Name).ToList();

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (this.ModelState.IsValid)
        {
            var task = new TodoTask
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                Status = model.Status,
                Assignee = model.Assignee,
                TodoListId = model.TodoListId,
            };

            await this.taskService.UpdateTaskAsync(task);
            await this.ProcessTagsOnTheFlyAsync(task.Id, model.TagNames);
            return this.RedirectToAction(nameof(this.Index), new { todoListId = model.TodoListId });
        }

        return this.View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var task = await this.taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        var model = new TodoTaskModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Assignee = task.Assignee,
            TodoListId = task.TodoListId,
        };

        return this.View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        await this.taskService.DeleteTaskAsync(id);
        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    [HttpGet]
    public async Task<IActionResult> Assigned(string? assignee, string? tagSearch, TodoTaskStatus? status, string? sortBy)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        if (assignee == null && tagSearch == null && status == null && sortBy == null)
        {
            assignee = this.User.Identity?.Name;
        }

        this.ViewBag.Assignee = assignee ?? string.Empty;
        this.ViewBag.TagSearch = tagSearch ?? string.Empty;
        this.ViewBag.CurrentStatus = status;
        this.ViewBag.CurrentSort = sortBy;

        var tasks = await this.taskService.GetAssignedTasksAsync(assignee, tagSearch, status, sortBy);

        var models = tasks.Select(t => new TodoTaskModel
        {
            Id = t.Id,
            Title = t.Title,
            DueDate = t.DueDate,
            Status = t.Status,
            Assignee = t.Assignee,
            TodoListId = t.TodoListId,
            Tags = t.Tags.Select(tg => new TodoTagModel { Id = tg.Id, Name = tg.Name }).ToList(),
        });

        var allTags = await this.tagService.GetAllTagsAsync();
        this.ViewBag.AllTags = allTags.Select(t => t.Name).ToList();

        return this.View(models);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTagsOnTheFly(int taskId, string? tagNames)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        await this.ProcessTagsOnTheFlyAsync(taskId, tagNames);
        return this.RedirectToAction(nameof(this.Details), new { id = taskId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, TodoTaskStatus newStatus, string assignee, TodoTaskStatus? currentStatus, string? currentSort, string? returnUr = null)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        await this.taskService.ChangeTaskStatusAsync(id, newStatus);

        if (!string.IsNullOrEmpty(returnUr) && this.Url.IsLocalUrl(returnUr))
        {
            return this.Redirect(returnUr);
        }

        return this.RedirectToAction(nameof(this.Assigned), new { assignee, status = currentStatus, sortBy = currentSort });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignTag(int taskId, int tagId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        await this.tagService.AssignTagToTaskAsync(taskId, tagId);
        return this.RedirectToAction(nameof(this.Details), new { id = taskId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveTag(int taskId, int tagId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        await this.tagService.RemoveTagFromTaskAsync(taskId, tagId);
        return this.RedirectToAction(nameof(this.Details), new { id = taskId });
    }

    private async Task ProcessTagsOnTheFlyAsync(int taskId, string? tagNamesString)
    {
        if (string.IsNullOrWhiteSpace(tagNamesString))
        {
            return;
        }

        var inputTags = tagNamesString.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(t => t.Trim().TrimStart('#'))
                                      .Distinct()
                                      .ToList();

        var allExistingTags = await this.tagService.GetAllTagsAsync();
        var currentTask = await this.taskService.GetTaskByIdAsync(taskId);
        var currentTaskTags = currentTask?.Tags ?? new List<TodoTag>();

        foreach (var tagName in inputTags)
        {
            var existingTag = allExistingTags.FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
            int tagId;

            if (existingTag == null)
            {
                var newTag = await this.tagService.CreateTagAsync(new TodoTag { Name = tagName });
                tagId = newTag.Id;
            }
            else
            {
                tagId = existingTag.Id;
            }

            if (!currentTaskTags.Any(t => t.Id == tagId))
            {
                await this.tagService.AssignTagToTaskAsync(taskId, tagId);
            }
        }
    }
}
