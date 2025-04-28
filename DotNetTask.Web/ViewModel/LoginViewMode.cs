using System.ComponentModel.DataAnnotations;

namespace DotNetTask.Web.ViewModel;

public class LoginViewModel : BaseViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }

}
