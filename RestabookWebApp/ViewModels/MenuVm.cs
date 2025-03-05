using RestabookWebApp.Models;

namespace RestabookWebApp.ViewModels;

public class MenuVm
{
    public List<Product> Products { get; set; }
    public List<Category> Categories { get; set; }
}