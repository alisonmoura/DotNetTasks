using DotNetTask.Data.Entities;

namespace DotNetTask.Services.Interfaces;

public interface ILoginService : IBaseService<ApplicationUser>
{
    public ApplicationUser? Login(ApplicationUser User);
}