using RestabookWebApp.Models;

namespace RestabookWebApp.ViewModels;

public class HomeVM
{
    public List<Slider> Sliders { get; set; }
    public About? About { get; set; }
    public List<Service> Services { get; set; }
    public List<Chef> Chefs { get; set; }
    public List<Event> Events { get; set; }
    public List<Product> Products { get; set; }
}