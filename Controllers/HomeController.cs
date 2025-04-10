using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetTask.Models;

namespace DotNetTask.Controllers;

public class HomeController(DotNetTaskDbContext db) : Controller
{
    private readonly DotNetTaskDbContext _db = db;

    public IActionResult Index([FromQuery] string? Title)
    {
        var tasks = SearchTasks(Title);
        return View(new HomeViewModel
        {
            Tasks = tasks,
            Count = tasks.Length,
            SearchTitle = Title ?? ""
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult New()
    {
        return RedirectToAction("Index", "CreateTaskController");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private HomeViewModelTask[] SearchTasks(string? SearchTitle)
    {
        var searchTitle = SearchTitle ?? "";
        var tasks = _db.Tasks
            .Where((t) => t.Title.Contains(searchTitle, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(t => t.DueDate);

        return tasks
            .Select(t => new HomeViewModelTask
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                DueDate = t.DueDate.ToString("yyyy/MM/dd")
            }).ToArray();
    }
}
