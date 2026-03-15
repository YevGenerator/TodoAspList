using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoTagController : Controller
{
    private readonly ITodoTagWebApiService tagService;

    public TodoTagController(ITodoTagWebApiService tagService)
    {
        this.tagService = tagService;
    }

    public async Task<IActionResult> Index()
    {
        var tags = await this.tagService.GetAllTagsAsync();
        var models = tags.Select(t => new TodoTagModel
        {
            Id = t.Id,
            Name = t.Name,
        });

        this.ViewBag.Tags = models;
        return this.View(new TodoTagModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoTagModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (this.ModelState.IsValid)
        {
            var tag = new TodoTag { Name = model.Name };
            await this.tagService.CreateTagAsync(tag);
            return this.RedirectToAction(nameof(this.Index));
        }

        var tags = await this.tagService.GetAllTagsAsync();
        this.ViewBag.Tags = tags.Select(t => new TodoTagModel { Id = t.Id, Name = t.Name });
        return this.View("Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await this.tagService.DeleteTagAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }
}
