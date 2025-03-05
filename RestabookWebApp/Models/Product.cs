using System.ComponentModel.DataAnnotations;
using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Product : BaseEntity
{
    public string? PhotoPath { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public DateOnly? Expires { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}