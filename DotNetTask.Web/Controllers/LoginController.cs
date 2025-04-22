using DotNetTask.Data.Entities;
using DotNetTask.Data.Resources;
using DotNetTask.Services.Exceptions;
using DotNetTask.Services.Interfaces;
using DotNetTask.Web.ViewModel;
using DotNetTask.Web.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTask.Web.Controllers;

public class LoginController(SignInManager<ApplicationUser> signInManager, ILoginService service) : Controller
{

    private readonly SignInManager<ApplicationUser> _signManager = signInManager;
    private readonly ILoginService _service = service;

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    [HttpGet]
    public IActionResult New()
    {
        return View("New", new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel ViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new BusinessException
                (
                    ValidationMessages.ValidationFailed,
                    MessageUtils.ExtractModelErrors(ModelState)
                );
            }

            var result = await _signManager.PasswordSignInAsync(ViewModel.Email, ViewModel.Password, false, false);
            if (!result.Succeeded) throw new BusinessException("User not found.");
            return RedirectToAction("Index", "Home");
        }
        catch (BusinessException ex)
        {
            ViewModel.Error = ex.Message;
            ViewModel.ShowError = true;
            ViewModel.ErrorFields = ex.Errors;
            return View("Index", ViewModel);
        }

    }
}