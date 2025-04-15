using DotNetTask.Data.Entities;
using DotNetTask.Services.Filters;

namespace DotNetTask.Services.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    IEnumerable<TaskItem> GetAllByFilterAsync(TaskItemFilter? filter = null);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<int?> GetLastIdAsync();
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(int id);
    Task MarkAsDoneAsync(int id);
    Task MarkAsCanceledAsync(int id);
}
