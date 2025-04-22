using Microsoft.AspNetCore.Mvc;
using DotNetTask.Data.Entities;
using DotNetTask.Data.Resources;
using DotNetTask.Services.Exceptions;
using DotNetTask.Services.Interfaces;
using DotNetTask.Web.Utils;
using DotNetTask.Web.ViewModel;

namespace DotNetTask.Web.Controllers;
public class NewAccountController(IUserService service) : Controller
{
    private readonly IUserService _service = service;

    [HttpGet]
    public IActionResult Index()
    {
        return View(new NewAccountViewModel());
    }

    [HttpPost]
    public IActionResult Create(NewAccountViewModel ViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new BusinessException(
                    ValidationMessages.ValidationFailed,
                    MessageUtils.ExtractModelErrors(ModelState)
                );
            }

            var user = new ApplicationUser()
            {
                Name = ViewModel.Name,
                Email = ViewModel.Email,
                Password = ViewModel.Password
            };

            var result = _service.AddAsync(user);

            if (result.Result.Succeeded)
            {
                return RedirectToAction("Index", "Login");
            }

            var passwordErrors = new List<string>();

            foreach (var error in result.Result.Errors)
            {
                Console.WriteLine(error);
                switch (error.Code)
                {
                    case "PasswordTooShort":
                    case "PasswordRequiresNonAlphanumeric":
                    case "PasswordRequiresLower":
                    case "PasswordRequiresUpper":
                        passwordErrors.Add(error.Description);
                        break;
                }
            }
            if (passwordErrors.Count > 0) ViewModel.ErrorFields.Add("Password", passwordErrors);
            ViewModel.ShowError = true;
            ViewModel.Error = ValidationMessages.ValidationFailed;
            return View("Index", ViewModel);
        }
        catch (BusinessException ex)
        {
            ViewModel.ShowError = true;
            ViewModel.Error = ex.Message;
            ViewModel.ErrorFields = ex.Errors;
            return View("Index", ViewModel);
        }
    }

}