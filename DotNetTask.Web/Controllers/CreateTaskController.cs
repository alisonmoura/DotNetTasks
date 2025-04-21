using Microsoft.AspNetCore.Mvc;
using DotNetTask.Data.Entities;
using DotNetTask.Data.Enums;
using DotNetTask.Data.Resources;
using DotNetTask.Web.ViewModel;
using DotNetTask.Services.Interfaces;
using DotNetTask.Services.Exceptions;
using DotNetTask.Web.Utils;

namespace DotNetTask.Web.Controllers;

public class CreateTaskController(ITaskService service) : Controller
{
    private readonly ITaskService _service = service;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int? lastId = await _service.GetLastIdAsync();

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
        await _service.MarkAsDoneAsync(Id ?? 0);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Cancel([FromRoute] int? Id)
    {
        await _service.MarkAsCanceledAsync(Id ?? 0);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] int? Id)
    {
        await _service.DeleteAsync(Id ?? 0);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int? Id)
    {
        var task = await _service.GetByIdAsync(Id ?? 0);
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
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = MessageUtils.ExtractModelErrors(ModelState);
                throw new BusinessException
                (
                    ValidationMessages.ValidationFailed,
                    errors
                );
            }

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
                await _service.UpdateAsync(task);
            }
            else
            {
                await _service.AddAsync(task);

            }
            return RedirectToAction("Index", "Home");
        }
        catch (BusinessException ex)
        {
            ViewModel.Error = ex.Message;
            ViewModel.ShowError = true;
            ViewModel.ErrorFields = ex.Errors;
            ViewModel.DueDateFmt = ViewModel.DueDate.ToString("yyyy-MM-dd");
            return View("Index", ViewModel);
        }
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
