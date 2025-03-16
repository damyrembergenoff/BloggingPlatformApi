namespace BloggingPLatformApi.Dtos;

public class User : DtoBase
{
    public required string Username { get; set; }
    public required string Role { get; set; }

    public ICollection<PostListItem> Posts { get; set; } = [];
    public ICollection<CommentListItem> Comments { get; set; } = [];
    public ICollection<LikeListItem> Likes { get; set; } = [];
    public ICollection<NotificationListItem> Notifications { get; set; } = [];
}

public class UserListItem : DtoBase
{
    public required string Username { get; set; }
    public required string Role { get; set; }
}

public class CreateOrLoginUser
{
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
}