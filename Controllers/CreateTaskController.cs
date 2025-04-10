using DotNetTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTask.Controllers;

public class CreateTaskController(DotNetTaskDbContext db) : Controller
{
    private readonly DotNetTaskDbContext _db = db;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int? lastId = await _db.Tasks.Select(t => t.Id).OrderDescending().FirstOrDefaultAsync();
        return View(new CreateTaskViewModel { Id = lastId.HasValue ? lastId.Value + 1 : 1, EditMode = false });
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int? Id)
    {
        Models.Task? task = await _db.Tasks.Where(t => t.Id == Id).FirstOrDefaultAsync();
        if (task != null)
        {
            return View("Index", new CreateTaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                DueDateFmt = task.DueDate.ToString("yyyy-MM-dd"),
                Description = task.Description,
                Status = task.Status,
                EditMode = true,
            });
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Save(CreateTaskViewModel ViewModel)
    {
        var task = new Models.Task
        {
            Title = ViewModel.Title,
            Description = ViewModel.Description,
            Status = PrepareStatus(ViewModel),
            DueDate = ViewModel.DueDate
        };

        if (ViewModel.Mode == "Edit")
        {
            task.Id = ViewModel.Id;
            _db.Update(task);
        }
        else
        {
            _db.Add(task);

        }
        await _db.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    private static TaskStatusEnum PrepareStatus(CreateTaskViewModel ViewModel)
    {
        TaskStatusEnum status;
        if (ViewModel.Mode == "Edit")
        {
            status = ViewModel.Status;
        }
        else if (ViewModel.MarkAsDone == "on")
        {
            status = TaskStatusEnum.DONE;
        }
        else if (DateOnly.FromDateTime(DateTime.Today) > ViewModel.DueDate)
        {
            status = TaskStatusEnum.OVERDUE;
        }
        else
        {
            status = TaskStatusEnum.PENDING;
        }
        return status;
    }
}
