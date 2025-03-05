using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class About : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? PhotoPath { get; set; }
}