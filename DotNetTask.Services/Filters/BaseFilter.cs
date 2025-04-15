namespace DotNetTask.Services.Filters;

public class BaseFilter
{
    public string? OrderBy { get; set; }
    public OrderDirectionEnum? OrderDirection { get; set; } = OrderDirectionEnum.ASC;
}
