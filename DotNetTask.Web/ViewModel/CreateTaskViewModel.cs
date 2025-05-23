using System.ComponentModel.DataAnnotations;
using DotNetTask.Data.Enums;
using DotNetTask.Data.Resources;

namespace DotNetTask.Web.ViewModel;

public class CreateTaskViewModel : BaseViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredTitle)]
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateOnly DueDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.PENDING;
    public string DueDateFmt { get; set; } = "";
    public string MarkAsDone { get; set; } = "";
    public bool EditMode { get; set; } = false;
    public string Mode { get; set; } = "New"; // New, Edit
    public Array StatusOptions { get; set; } = Enum.GetValues(typeof(TaskStatusEnum));
}