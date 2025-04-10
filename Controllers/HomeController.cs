using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetTask.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTask.Controllers;

public class HomeController(DotNetTaskDbContext db) : Controller
{
    private readonly DotNetTaskDbContext _db = db;

    public IActionResult Index()
    {
        var tasks = _db.Tasks
        .OrderBy(t => t.DueDate)
        .Select(t => new HomeViewModelTask
        {
            Id = t.Id,
            Title = t.Title,
            Status = t.Status,
            DueDate = t.DueDate.ToString("yyyy/MM/dd")
        }).ToArray();
        return View(new HomeViewModel { Tasks = tasks, Count = tasks.Length });
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
}
