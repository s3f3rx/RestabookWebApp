using Microsoft.AspNetCore.Identity;

namespace RestabookWebApp.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhotoPath { get; set; }
}