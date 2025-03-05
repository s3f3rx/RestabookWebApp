using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Tag : BaseEntity
{
    public string Name { get; set; }

    // Many-to-Many 
    public ICollection<ArticleTag>? ArticleTags { get; set; }
}