using RestabookWebApp.Models;

namespace RestabookWebApp.ViewModels;

public class AboutVm
{
    public About? About { get; set; }
    public List<Chef> Chefs { get; set; }
}