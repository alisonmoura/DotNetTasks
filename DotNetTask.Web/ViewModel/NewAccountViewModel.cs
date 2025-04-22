using System.ComponentModel.DataAnnotations;
using DotNetTask.Data.Resources;

namespace DotNetTask.Web.ViewModel;

public class NewAccountViewModel : BaseViewModel
{

    [Required(ErrorMessage = ValidationMessages.RequiredName)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = ValidationMessages.RequiredEmail)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = ValidationMessages.RequiredPassword)]
    public string Password { get; set; } = "";

}