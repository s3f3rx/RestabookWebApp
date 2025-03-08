using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models;

public class Article : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? PhotoPath { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }

    //navigation
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    //One-to-Many
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    // Many-to-Many 
    public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
    public ICollection<ArticleComment> ArticleComments { get; set; } = new List<ArticleComment>();
}