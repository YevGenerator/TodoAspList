using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly ITodoTaskWebApiService taskService;
    private readonly ITodoListWebApiService listService;

    public HomeController(ILogger<HomeController> logger, ITodoTaskWebApiService taskService, ITodoListWebApiService listService)
    {
        this.logger = logger;
        this.taskService = taskService;
        this.listService = listService;
    }

    public async Task<IActionResult> Index()
    {
        if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
        {
            var userName = this.User.Identity.Name ?? string.Empty;
            var myTasks = await this.taskService.GetAssignedTasksAsync(userName, null);

            var myLists = await this.listService.GetTodoListsAsync();
            this.ViewBag.ListNames = myLists.ToDictionary(l => l.Id, l => l.Title);

            var today = DateTime.Today;

            var activeTasks = myTasks.Where(t => t.Status != TodoListApp.Models.Enums.TodoTaskStatus.Completed).ToList();

            var overdueTasks = activeTasks
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date < today)
                .OrderBy(t => t.DueDate)
                .ToList();

            var dueSoonTasks = activeTasks
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date >= today && t.DueDate.Value.Date <= today.AddDays(3))
                .OrderBy(t => t.DueDate)
                .ToList();

            var otherTasks = activeTasks
                .Except(overdueTasks)
                .Except(dueSoonTasks)
                .OrderByDescending(t => t.Id)
                .ToList();

            this.ViewBag.OverdueTasks = overdueTasks;
            this.ViewBag.DueSoonTasks = dueSoonTasks;
            this.ViewBag.OtherTasks = otherTasks;
        }

        return this.View();
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
