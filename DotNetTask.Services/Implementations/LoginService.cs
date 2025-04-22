using DotNetTask.Data;
using DotNetTask.Data.Entities;
using DotNetTask.Services.Interfaces;

namespace DotNetTask.Services.Implementations;

public class LoginService(ApplicationDbContext context) : ILoginService
{
    private readonly ApplicationDbContext _context = context;
    public ApplicationUser? Login(ApplicationUser User)
    {
        return _context.Users
        .Where(u => u.Email == User.Email)
        .Where(u => u.Password == User.Password)
        .FirstOrDefault();
    }

    public void Validate(ApplicationUser model)
    {
        throw new NotImplementedException();
    }
}