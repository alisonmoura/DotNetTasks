namespace DotNetTask.Data.Entities;

public class TaskItem
{
    public int? Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.PENDING;
    public DateOnly DueDate { get; set; }
    public DateTime CreatedAt { get; set; }

}