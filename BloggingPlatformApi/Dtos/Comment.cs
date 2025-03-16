namespace BloggingPLatformApi.Dtos;

public class Comment : DtoBase
{
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }

    public PostListItem Post { get; set; } = default!;
    public UserListItem User { get; set; } = default!;
}

public class CommentListItem : DtoBase
{
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}

public class CreateComment
{
    public string Content { get; set; } = default!;
}