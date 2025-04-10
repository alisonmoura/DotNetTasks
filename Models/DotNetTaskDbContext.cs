using Microsoft.EntityFrameworkCore;

namespace DotNetTask.Models;

public class DotNetTaskDbContext : DbContext
{

    public DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("DotNetTask");
    }

}