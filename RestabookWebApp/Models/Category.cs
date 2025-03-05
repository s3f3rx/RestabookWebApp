using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Product>? Products { get; set; }

    // Bir categorynin birden çox Article'ı ola bilər (One-to-Many)
    public ICollection<Article>? Articles { get; set; }
}