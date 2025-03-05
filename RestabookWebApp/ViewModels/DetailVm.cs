using RestabookWebApp.Models;

namespace RestabookWebApp.ViewModels;

public class DetailVm
{
    public Product Product { get; set; }
    public List<Category> Categories { get; set; }
    public List<Category> PopularCategories { get; set; } 
    public List<Product> PopularProducts { get; set; } 
    public List<Product> RelatedProducts  { get; set; } 

    
}