using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class ArticleComment : BaseEntity
{
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public int ArticleId { get; set; }
    public Article Article { get; set; }
    public string Comment { get; set; }
}