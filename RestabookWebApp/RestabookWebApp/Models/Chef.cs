using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Chef : BaseEntity
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? PhotoPath { get; set; }
}