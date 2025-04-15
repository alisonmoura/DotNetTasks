using DotNetTask.Data.Enums;

namespace DotNetTask.Web.ModelView;

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
    public string SearchStatus { get; set; } = "";
    public Array StatusOptions { get; set; } = Enum.GetValues(typeof(TaskStatusEnum));
}
