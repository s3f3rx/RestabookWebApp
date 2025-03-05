using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Service:BaseEntity
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string? PhotoPath { get; set; }
    public string Description { get; set; }
}