using System.ComponentModel.DataAnnotations;

namespace DotNetTask.Web.ViewModel;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }

    public bool ShowError { get; set; } = false;

    public string Error { get; set; } = "";

    public Dictionary<string, List<string>> ErrorFields { get; set; } = [];
}
