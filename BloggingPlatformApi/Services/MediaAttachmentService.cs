using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Data;
using BloggingPLatformApi.Dtos;
using BloggingPLatformApi.Hubs;
using BloggingPLatformApi.Mappers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BloggingPLatformApi.Services;

public class MediaAttachmentService(AppDbContext dbContext, 
    IUserService userService,
    INotificationService notificationService,
    IHubContext<NotificationHub> hubContext) : IMediaAttachmentService
{
    public async ValueTask UploadMediaAsync(Guid userId, Guid postId, CreateMedia media, CancellationToken cancellationToken = default)
    {
        var post = await dbContext.Posts
            .Include(p => p.MediaAttachments)
            .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
        
        if (post is null)
            throw new KeyNotFoundException($"Post with id {postId} not found.");
        
        if (post.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to upload media for this post.");

        var mediaEntity = media.ToEntity();
        mediaEntity.PostId = postId;
        
        dbContext.MediaAttachments.Add(mediaEntity);

        string notificationMessage = "Media muvaffaqiyatli yuklandi";
        await hubContext.Clients.User(userId.ToString()).SendAsync(notificationMessage, cancellationToken);

        await notificationService.SendNotificationAsync(userId, notificationMessage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteMediaAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken = default)
    {
        var media = await dbContext.MediaAttachments
            .Include(x => x.Post)
            .FirstOrDefaultAsync(p => p.Id == mediaId, cancellationToken);
            
        if (media is null)
            throw new KeyNotFoundException($"Media with id {mediaId} not found.");

        var userRole = await userService.GetUserRoleAsync(userId);
        var post = media.Post;

        if (post.UserId != userId && userRole != "Admin")
            throw new UnauthorizedAccessException("You are not authorized to delete media for this post.");

        if (post.MediaAttachments == null || !post.MediaAttachments.Any())
            throw new KeyNotFoundException("No media attachments found for this post.");
        
        post.MediaAttachments.Remove(media);

        string notificationMessage = "Media muvaffaqiyatli ochirildi";
        await hubContext.Clients.User(userId.ToString()).SendAsync(notificationMessage, cancellationToken);

        await notificationService.SendNotificationAsync(userId, notificationMessage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
