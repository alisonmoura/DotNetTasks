using DotNetTask.Data;
using DotNetTask.Data.Entities;
using DotNetTask.Data.Enums;
using DotNetTask.Services.Interfaces;
using DotNetTask.Services.Filters;
using Microsoft.EntityFrameworkCore;
using DotNetTask.Services.Exceptions;

namespace DotNetTask.Services.Implementations;

public class TaskService(ApplicationDbContext context) : ITaskService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<TaskItem>> GetAllAsync() =>
        await _context.TaskItems.AsNoTracking().ToListAsync();

    public IEnumerable<TaskItem> GetAllByFilterAsync(TaskItemFilter? filter = null)
    {
        var query = ApplyOrdering(_context.TaskItems.AsQueryable().AsNoTracking(), filter);

        if (filter is not null)
        {
            if (filter.Id.HasValue)
                query = query.Where(t => t.Id == filter.Id);

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(t => EF.Functions.Like(t.Title, $"%{filter.Title}%"));

            if (!string.IsNullOrWhiteSpace(filter.Description))
                query = query.Where(t => t.Description != null && EF.Functions.Like(t.Status.ToString(), $"%{filter.Description}%"));

            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == filter.Status);

            if (filter.DueDate.HasValue)
                query = query.Where(t => t.DueDate == filter.DueDate);

            if (filter.CreatedAt.HasValue)
                query = query.Where(t => t.CreatedAt.Date == filter.CreatedAt.Value.Date);
        }

        return [.. query];
    }

    public async Task<TaskItem?> GetByIdAsync(int id) =>
        await _context.TaskItems.AsNoTracking().Where(t => t.Id == id).FirstAsync();

    public async Task AddAsync(TaskItem task)
    {
        validate(task);
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        var count = await _context.TaskItems.AsNoTracking().Where(t => t.Id == task.Id).CountAsync();
        if (count == 1)
        {
            validate(task);
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
        }
        else throw new BusinessException("Task not found.");
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

    async public Task MarkAsDoneAsync(int id)
    {
        TaskItem? task = await _context.TaskItems.Where(t => t.Id == id).FirstOrDefaultAsync();
        if (task != null)
        {
            task.Status = TaskStatusEnum.DONE;
            _context.Update(task);
            await _context.SaveChangesAsync();
        }
    }

    async public Task MarkAsCanceledAsync(int id)
    {
        TaskItem? task = await _context.TaskItems.Where(t => t.Id == id).FirstOrDefaultAsync();
        if (task != null)
        {
            task.Status = TaskStatusEnum.CANCELED;
            _context.Update(task);
            await _context.SaveChangesAsync();
        }
    }

    async Task<int?> ITaskService.GetLastIdAsync()
    {
        return await _context.TaskItems
            .AsNoTracking()
            .OrderByDescending(t => t.Id)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();
    }

    public void validate(TaskItem task)
    {
        if (string.IsNullOrEmpty(task.Title))
            throw new BusinessException("The title must be informed.", new Dictionary<string, bool> { ["title"] = true });
    }

    public static IQueryable<TaskItem> ApplyOrdering(IQueryable<TaskItem> query, BaseFilter? filter)
    {
        if (filter is null)
            return query;

        if (string.IsNullOrEmpty(filter.OrderBy))
            return query;

        var descending = filter.OrderDirection == OrderDirectionEnum.DESC;

        return filter.OrderBy.ToLower() switch
        {
            "title" => descending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
            "status" => descending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
            "duedate" => descending ? query.OrderByDescending(x => x.DueDate) : query.OrderBy(x => x.DueDate),
            "createdat" => descending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            _ => query
        };
    }

}
