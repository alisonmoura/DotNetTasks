using Microsoft.AspNetCore.Identity;

namespace DotNetTask.Data.Entities;

public class ApplicationUser : IdentityUser
{
    // Add custom properties here if needed
    public string Name { get; set; } = "";

    public string Password { get; set; } = "";
}
