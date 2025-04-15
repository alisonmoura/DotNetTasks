using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetTask.Web.ModelView;
using DotNetTask.Services.Filters;
using DotNetTask.Services.Interfaces;

namespace DotNetTask.Web.Controllers;

public class HomeController(ITaskService service) : Controller
{
    private readonly ITaskService _service = service;

    public IActionResult Index([FromQuery] TaskItemFilter filter)
    {
        filter.OrderBy = "duedate";
        var tasks = _service.GetAllByFilterAsync(filter)
            .Select(t => new HomeViewModelTask
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                DueDate = t.DueDate.ToString("yyyy/MM/dd")
            }).ToArray();

        return View(new HomeViewModel
        {
            Tasks = tasks,
            Count = tasks.Length,
            SearchTitle = filter.Title ?? "",
            SearchStatus = filter.Status.ToString() ?? ""
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

    // private HomeViewModelTask[] SearchTasks(string? SearchTitle, string? SearchStatus)
    // {
    //     var tasks = _db.TaskItems
    //         .Where((t) => SearchTitle == "" || EF.Functions.Like(t.Title, $"%{SearchTitle}%"))
    //         .Where((t) => SearchStatus == "" || EF.Functions.Like(t.Status.ToString(), $"%{SearchStatus}%"))
    //         .OrderBy(t => t.DueDate);

    //     return tasks
    //         .Select(t => new HomeViewModelTask
    //         {
    //             Id = t.Id,
    //             Title = t.Title,
    //             Status = t.Status,
    //             DueDate = t.DueDate.ToString("yyyy/MM/dd")
    //         }).ToArray();
    // }
}
