using BloggingPLatformApi.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPLatformApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController(IAuthenticationStateService authenticationStateService, INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Author,Admin")]
    public async ValueTask<IActionResult> GetNotifications(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(authenticationStateService.UserId))
            return Unauthorized();

        var userId = Guid.Parse(authenticationStateService.UserId);
        var notifications = await notificationService.GetNotificationsAsync(userId, cancellationToken);

        return Ok(notifications);
    }

    [HttpPost("read/{notificationId}")]
    [Authorize(Roles = "Author")]
    public async ValueTask<IActionResult> MarkNotificationAsRead(Guid notificationId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(authenticationStateService.UserId))
            return Unauthorized();
        
        var userId = Guid.Parse(authenticationStateService.UserId);
        await notificationService.MarkNotificationAsReadAsync(userId, notificationId, cancellationToken);
        return NoContent();
    }

    [HttpPost("send")]
    [Authorize(Roles = "Author")]
    public async ValueTask<IActionResult> SendNotification([FromBody] string message, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(authenticationStateService.UserId))
            return Unauthorized();
        
        var userId = Guid.Parse(authenticationStateService.UserId);

        await notificationService.SendNotificationAsync(userId, message, cancellationToken);
        return Created();
    }
}