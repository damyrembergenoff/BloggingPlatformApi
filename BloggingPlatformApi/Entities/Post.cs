namespace BloggingPLatformApi.Entities;

public class Post : EntityBase
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }

    public User? User { get; set; }
    public Category? Category { get; set; } 
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<Like> Likes { get; set; } = [];
    public ICollection<MediaAttachment> MediaAttachments { get; set; } = [];
    public ICollection<PostTag> PostTags { get; set; } = [];
}
