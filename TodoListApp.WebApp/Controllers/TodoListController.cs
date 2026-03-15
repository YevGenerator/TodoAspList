using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoListController : Controller
{
    private readonly ITodoListWebApiService todoListService;

    public TodoListController(ITodoListWebApiService todoListService)
    {
        this.todoListService = todoListService;
    }

    public async Task<IActionResult> Index()
    {
        var lists = await this.todoListService.GetTodoListsAsync();
        var models = lists.Select(l => new TodoListModel
        {
            Id = l.Id,
            Title = l.Title,
            Description = l.Description,
        });

        return this.View(models);
    }

    public IActionResult Create()
    {
        return this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (this.ModelState.IsValid)
        {
            var todoList = new TodoList
            {
                Title = model.Title,
                Description = model.Description,
            };

            await this.todoListService.CreateTodoListAsync(todoList);
            return this.RedirectToAction(nameof(this.Index));
        }

        return this.View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var list = await this.todoListService.GetTodoListByIdAsync(id);
        if (list == null)
        {
            return this.NotFound();
        }

        var model = new TodoListModel
        {
            Id = list.Id,
            Title = list.Title,
            Description = list.Description,
        };

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (this.ModelState.IsValid)
        {
            var todoList = new TodoList
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
            };

            await this.todoListService.UpdateTodoListAsync(todoList);
            return this.RedirectToAction(nameof(this.Index));
        }

        return this.View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var list = await this.todoListService.GetTodoListByIdAsync(id);
        if (list == null)
        {
            return this.NotFound();
        }

        var model = new TodoListModel
        {
            Id = list.Id,
            Title = list.Title,
            Description = list.Description,
        };

        return this.View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await this.todoListService.DeleteTodoListAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }
}
