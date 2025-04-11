using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetTask.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTask.Controllers;

public class HomeController(DotNetTaskDbContext db) : Controller
{
    private readonly DotNetTaskDbContext _db = db;

    public IActionResult Index([FromQuery] string? Title, [FromQuery] string? Status)
    {
        var tasks = SearchTasks(Title, Status);
        return View(new HomeViewModel
        {
            Tasks = tasks,
            Count = tasks.Length,
            SearchTitle = Title ?? "",
            SearchStatus = Status ?? ""
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

    private HomeViewModelTask[] SearchTasks(string? SearchTitle, string? SearchStatus)
    {
        var tasks = _db.Tasks
            .Where((t) => SearchTitle == "" || EF.Functions.Like(t.Title, $"%{SearchTitle}%"))
            .Where((t) => SearchStatus == "" || EF.Functions.Like(t.Status.ToString(), $"%{SearchStatus}%"))
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
