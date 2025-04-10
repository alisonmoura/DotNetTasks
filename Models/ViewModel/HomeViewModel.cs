namespace DotNetTask.Models;

public class HomeViewModelTask
{
    public int? Id { get; set; }
    public string Title { get; set; } = "";
    public TaskStatusEnum? Status { get; set; }
    public string DueDate { get; set; } = "";
}

public class HomeViewModel
{
    public int Count { get; set; }
    public required HomeViewModelTask[] Tasks { get; set; }
    public string SearchTitle { get; set; } = "";
}
