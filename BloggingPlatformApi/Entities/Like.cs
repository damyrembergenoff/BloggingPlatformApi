namespace BloggingPLatformApi.Entities;

public class Like : EntityBase
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }

    public Post Post { get; set; } = default!;
    public User User { get; set; } = default!;
}
