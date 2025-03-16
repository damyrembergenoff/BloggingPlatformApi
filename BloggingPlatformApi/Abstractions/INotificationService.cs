namespace BloggingPLatformApi.Abstractions;

public interface INotificationService
{
    ValueTask<IEnumerable<Dtos.NotificationListItem>> GetNotificationsAsync(Guid userId, CancellationToken cancellationToken = default);
    ValueTask MarkNotificationAsReadAsync(Guid userId, Guid notificationId, CancellationToken cancellationToken = default);
    ValueTask SendNotificationAsync(Guid userId, string message, CancellationToken cancellationToken = default);
}