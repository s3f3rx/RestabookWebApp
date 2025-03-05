using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class ArticleTag : BaseEntity
{
    public int ArticleId { get; set; }
    //navigation; connection with Article
    public Article Article { get; set; }

    public int TagId { get; set; }
    //navigation; connection with Tag
    public Tag Tag { get; set; }
}
