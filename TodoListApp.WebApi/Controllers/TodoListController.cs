using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Interfaces;
using TodoListApp.Models;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoListDatabaseService todoListService;

    public TodoListController(ITodoListDatabaseService todoListService)
    {
        this.todoListService = todoListService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListModel>>> Get()
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return this.Unauthorized();
        }

        var lists = await this.todoListService.GetTodoListsAsync(userId);
        var models = lists.Select(l => new TodoListModel
        {
            Id = l.Id,
            Title = l.Title,
            Description = l.Description,
            Tasks = l.Tasks.Select(t => new TodoTaskModel
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                DueDate = t.DueDate,
                Assignee = t.Assignee,
                TodoListId = t.TodoListId,
                Tags = t.Tags.Select(tg => new TodoTagModel { Id = tg.Id, Name = tg.Name }).ToList(),
            }).ToList(),
        });

        return this.Ok(models);
    }

    [HttpPost]
    public async Task<ActionResult<TodoListModel>> Post([FromBody] TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return this.Unauthorized();
        }

        var todoList = new TodoList
        {
            Title = model.Title,
            Description = model.Description,
            OwnerId = userId,
        };

        var createdList = await this.todoListService.AddTodoListAsync(todoList);
        model.Id = createdList.Id;

        return this.CreatedAtAction(nameof(this.Get), new { id = model.Id }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (id != model.Id || !this.ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var todoList = new TodoList
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
        };

        await this.todoListService.UpdateTodoListAsync(todoList);
        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await this.todoListService.DeleteTodoListAsync(id);
        return this.NoContent();
    }
}
