using Microsoft.AspNetCore.SignalR;

namespace BloggingPLatformApi.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToUser(string userId, string message)
        => await Clients.User(userId).SendAsync("ReceiveNotification", message);

    public async Task BroadcastNotification(string message)
        => await Clients.All.SendAsync("ReceiveNotification", message);
}