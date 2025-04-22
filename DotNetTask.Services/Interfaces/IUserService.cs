using DotNetTask.Data.Entities;
using DotNetTask.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotNetTask.Services.Interfaces;

public interface IUserService : IBaseService<ApplicationUser>
{
    public Task<IdentityResult> AddAsync(ApplicationUser User);
}