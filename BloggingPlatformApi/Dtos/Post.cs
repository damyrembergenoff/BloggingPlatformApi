namespace BloggingPLatformApi.Dtos;

public class Post : DtoBase
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }

    public UserListItem? User { get; set; }
    public CategoryListItem? Category { get; set; }
    public ICollection<CommentListItem> Comments { get; set; } = [];
    public ICollection<LikeListItem> Likes { get; set; } = [];
    public ICollection<MediaAttachmentListItem> MediaAttachments { get; set; } = [];
    public ICollection<PostTagListItem> PostTags { get; set; } = [];
}

public class PostListItem : DtoBase
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }   
}

public class CreatePost
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Status { get; set; }
    public Guid CategoryId { get; set; }
    public IList<Guid>? TagIds { get; set; }
}

public class UpdatePost
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid CategoryId { get; set; }
    public IList<Guid>? TagIds { get; set; }
}