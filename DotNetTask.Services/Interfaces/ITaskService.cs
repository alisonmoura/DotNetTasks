using DotNetTask.Data.Entities;

namespace DotNetTask.Services.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task AddAsync(TaskItem task);
    Task DeleteAsync(int id);
}
