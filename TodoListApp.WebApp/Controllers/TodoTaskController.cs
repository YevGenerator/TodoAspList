using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using TodoListApp.Models.Enums;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

public class TodoTaskController : Controller
{
    private readonly ITodoTaskWebApiService taskService;

    public TodoTaskController(ITodoTaskWebApiService taskService)
    {
        this.taskService = taskService;
    }

    public async Task<IActionResult> Index(int todoListId)
    {
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

    public async Task<IActionResult> Details(int id)
    {
        var task = await this.taskService.GetTaskByIdAsync(id);
        if (task == null) return this.NotFound();

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

    public IActionResult Create(int todoListId)
    {
        var model = new TodoTaskModel { TodoListId = todoListId };
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

            await this.taskService.CreateTaskAsync(task);
            return this.RedirectToAction(nameof(this.Index), new { todoListId = model.TodoListId });
        }

        return this.View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
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
            return this.RedirectToAction(nameof(this.Index), new { todoListId = model.TodoListId });
        }

        return this.View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
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
        await this.taskService.DeleteTaskAsync(id);
        return this.RedirectToAction(nameof(this.Index), new { todoListId = todoListId });
    }

    public async Task<IActionResult> Assigned(string assignee, TodoTaskStatus? status, string? sortBy)
    {
        this.ViewBag.Assignee = assignee;
        this.ViewBag.CurrentStatus = status;
        this.ViewBag.CurrentSort = sortBy;

        if (string.IsNullOrWhiteSpace(assignee))
        {
            return this.View(Enumerable.Empty<TodoTaskModel>());
        }

        var tasks = await this.taskService.GetAssignedTasksAsync(assignee, status, sortBy);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, TodoTaskStatus newStatus, string assignee, TodoTaskStatus? currentStatus, string? currentSort)
    {
        await this.taskService.ChangeTaskStatusAsync(id, newStatus);
        return this.RedirectToAction(nameof(this.Assigned), new { assignee = assignee, status = currentStatus, sortBy = currentSort });
    }
}
