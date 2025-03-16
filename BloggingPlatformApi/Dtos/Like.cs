namespace BloggingPLatformApi.Dtos;

public class Like : DtoBase
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }

    public PostListItem Post { get; set; } = default!;
    public UserListItem User { get; set; } = default!;
}

public class LikeListItem : DtoBase
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}