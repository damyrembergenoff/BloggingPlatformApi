namespace BloggingPLatformApi.Entities;

public class Notification : EntityBase
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = default!;
}