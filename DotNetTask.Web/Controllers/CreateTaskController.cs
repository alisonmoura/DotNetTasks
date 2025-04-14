using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetTask.Data;
using DotNetTask.Data.Entities;
using DotNetTask.Web.ModelView;

namespace DotNetTask.Web.Controllers;

public class CreateTaskController(ApplicationDbContext db) : Controller
{
    private readonly ApplicationDbContext _db = db;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int? lastId = await _db.TaskItems
            .AsNoTracking()
            .OrderByDescending(t => t.Id)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();

        return View(new CreateTaskViewModel
        {
            Id = lastId.HasValue ? lastId.Value + 1 : null,
            EditMode = false,
            DueDateFmt = DateTime.Now.ToString("yyyy-MM-dd"),
        });
    }

    [HttpGet]
    public async Task<IActionResult> Done([FromRoute] int? Id)
    {
        TaskItem? task = await _db.TaskItems.Where(t => t.Id == Id).FirstOrDefaultAsync();
        if (task != null)
        {
            task.Status = TaskStatusEnum.DONE;
            _db.Update(task);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Cancel([FromRoute] int? Id)
    {
        TaskItem? task = await _db.TaskItems.Where(t => t.Id == Id).FirstOrDefaultAsync();
        if (task != null)
        {
            task.Status = TaskStatusEnum.CANCELED;
            _db.Update(task);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] int? Id)
    {
        TaskItem? task = await _db.TaskItems.Where(t => t.Id == Id).FirstOrDefaultAsync();
        if (task != null)
        {
            _db.Remove(task);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int? Id)
    {
        TaskItem? task = await _db.TaskItems.Where(t => t.Id == Id).FirstOrDefaultAsync();
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
        var task = new TaskItem
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
