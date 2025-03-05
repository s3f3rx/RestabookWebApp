using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.ViewModels;

public class UserRoleVm
{
    public AppUser AppUser { get; set; }
    
    public List<string> Roles { get; set; }
}