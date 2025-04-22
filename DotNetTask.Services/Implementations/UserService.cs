
using Microsoft.AspNetCore.Identity;
using DotNetTask.Data;
using DotNetTask.Data.Entities;
using DotNetTask.Data.Resources;
using DotNetTask.Services.Exceptions;
using DotNetTask.Services.Interfaces;


namespace DotNetTask.Services.Implementations;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{

    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IdentityResult> AddAsync(ApplicationUser user)
    {
        if (user.UserName is null && user.Email is not null)
        {
            user.UserName = user.Email;
        }
        Validate(user);
        return await _userManager.CreateAsync(user, user.Password);
    }

    public void Validate(ApplicationUser model)
    {
        var errors = new Dictionary<string, List<string>>();
        var nameErrors = new List<string>();

        if (string.IsNullOrEmpty(model.Name))
            nameErrors.Add(ValidationMessages.RequiredName);
        if (!string.IsNullOrEmpty(model.Name) && model.Name.Length < 5)
            nameErrors.Add(ValidationMessages.MinLengthTitle);
        if (nameErrors.Count > 0) errors.Add("Name", nameErrors);

        if (errors.Count > 0)
        {
            throw new BusinessException
            (
                ValidationMessages.ValidationFailed,
                errors
            );
        }
    }
}