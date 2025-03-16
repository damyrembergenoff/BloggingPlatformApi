namespace BloggingPLatformApi.Entities;

public class Comment : EntityBase
{
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }

    public Post Post { get; set; } = default!;
    public User User { get; set; } = default!;
}
