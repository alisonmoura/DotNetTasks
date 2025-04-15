using DotNetTask.Data.Enums;

namespace DotNetTask.Services.Filters;

public class TaskItemFilter : BaseFilter
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatusEnum? Status { get; set; }
    public DateOnly? DueDate { get; set; }
    public DateTime? CreatedAt { get; set; }
}
