using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Event : BaseEntity
{
    public string Name { get; set; }
    public DateOnly EventDate { get; set; }
    public string Description { get; set; }
}