namespace DotNetTask.Models;

public class CreateTaskViewModel
{
    public int? Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateOnly DueDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.PENDING;
    public string DueDateFmt { get; set; } = "";
    public string MarkAsDone { get; set; } = "";
    public bool EditMode { get; set; } = false;
    public string Mode { get; set; } = "New"; // New, Edit
}