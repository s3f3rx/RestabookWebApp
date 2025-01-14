using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Models;

namespace RestabookWebApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Slider> Sliders { get; set; }
    public DbSet<About> Abouts { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Chef> Chefs { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}