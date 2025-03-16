namespace BloggingPLatformApi.Dtos;

public class Notification : DtoBase
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    public UserListItem User { get; set; } = default!;
}

public class NotificationListItem : DtoBase
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }
}