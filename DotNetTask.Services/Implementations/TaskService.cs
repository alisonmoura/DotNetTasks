using DotNetTask.Data;
using DotNetTask.Data.Entities;
using DotNetTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotNetTask.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync() => await _context.TaskItems.ToListAsync();

    public async Task<TaskItem?> GetByIdAsync(int id) => await _context.TaskItems.FindAsync(id);

    public async Task AddAsync(TaskItem task)
    {
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        var taskFound = await _context.TaskItems.FindAsync(task.Id);
        if (taskFound != null)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task != null)
        {
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
