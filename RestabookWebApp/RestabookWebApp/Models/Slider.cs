using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Slider : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? PhotoPath { get; set; }
}