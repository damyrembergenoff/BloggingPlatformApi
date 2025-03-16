using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Data;
using BloggingPLatformApi.Dtos;
using BloggingPLatformApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace BloggingPLatformApi.Services;

public class NotificationService(AppDbContext dbContext, IUserService userService) : INotificationService
{
    public async ValueTask<IEnumerable<NotificationListItem>> GetNotificationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = dbContext.Notifications.AsNoTracking();

        if(await userService.GetUserRoleAsync(userId, cancellationToken) == "Admin")
            return await notifications.Select(x => x.ToListItemDto()).ToListAsync(cancellationToken);
        
        notifications = notifications.Where(x => x.UserId == userId && x.IsRead == false);

        return await notifications.Select(x => x.ToListItemDto()).ToListAsync(cancellationToken);
    }

    public async ValueTask MarkNotificationAsReadAsync(Guid userId, Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId, cancellationToken);

        if(notification is null)
            throw new KeyNotFoundException($"Notification with {notificationId} not found");

        if(notification.UserId != userId)
            throw new UnauthorizedAccessException($"You are not authorized.");

        notification.IsRead = true;
    }

    public async ValueTask SendNotificationAsync(Guid userId, string message, CancellationToken cancellationToken = default)
    {
        var notification = new Entities.Notification
        {
            UserId = userId,
            Message = message
        };

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
