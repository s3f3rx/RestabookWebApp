using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Contact : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Phone { get; set; }
    public string Subject { get; set; }
    public string? Message { get; set; }
}