using Microsoft.AspNetCore.Mvc;
using TodoListApp.Interfaces;
using TodoListApp.Models;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskDatabaseService taskService;

    public TodoTaskController(ITodoTaskDatabaseService taskService)
    {
        this.taskService = taskService;
    }

    [HttpGet("list/{todoListId}")]
    public async Task<ActionResult<IEnumerable<TodoTaskModel>>> GetByListId(int todoListId)
    {
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

        return this.Ok(models);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTaskModel>> Get(int id)
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

        return this.Ok(model);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTaskModel>> Post([FromBody] TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var task = new TodoTask
        {
            Title = model.Title,
            Description = model.Description,
            DueDate = model.DueDate,
            Status = model.Status,
            Assignee = model.Assignee,
            TodoListId = model.TodoListId,
        };

        var createdTask = await this.taskService.AddTaskAsync(task);
        model.Id = createdTask.Id;

        return this.CreatedAtAction(nameof(this.Get), new { id = model.Id }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (id != model.Id || !this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

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
        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await this.taskService.DeleteTaskAsync(id);
        return this.NoContent();
    }
}
