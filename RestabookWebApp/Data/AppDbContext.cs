using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Models;

namespace RestabookWebApp.Data;

public class AppDbContext : IdentityDbContext<AppUser>
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
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ArticleTag> ArticleTags { get; set; }
    public DbSet<ArticleComment> ArticleComments { get; set; }
    
}