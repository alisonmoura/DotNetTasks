using DotNetTask.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTask.Web.Controllers;
public class NewAccountController : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        return View(new NewAccountViewModel());
    }

}