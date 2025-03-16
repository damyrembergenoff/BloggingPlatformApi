using Microsoft.EntityFrameworkCore;
using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Data;
using BloggingPLatformApi.Dtos;
using BloggingPLatformApi.Mappers;
using Microsoft.AspNetCore.SignalR;
using BloggingPLatformApi.Hubs;

namespace BloggingPLatformApi.Services;

public class CommentService(AppDbContext dbContext, 
        IUserService userService, 
        INotificationService notificationService,
        IHubContext<NotificationHub> hubContext) 
        : ICommentService
{
    public async ValueTask AddCommentAsync(Guid userId, Guid postId, CreateComment comment, CancellationToken cancellationToken = default)
    {
        var postExists = await dbContext.Posts.AnyAsync(x => x.Id == postId && x.UserId == userId, cancellationToken);
    
        if (!postExists)
            throw new KeyNotFoundException($"Post with ID {postId} not found.");

        var commentEntity = comment.ToEntity(userId);
        commentEntity.PostId = postId;
        
        var entry = dbContext.Comments.Add(commentEntity);
        await notificationService.SendNotificationAsync(userId, comment.Content, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        if(entry.Entity.UserId != userId)
        {
            string notificationMessage = "Sizning postingizga yangi izoh qoshildi";
            
            await hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notificationMessage, cancellationToken);
        }

        await hubContext.Clients.All.SendAsync("ReceiveNotification", "Sizning postingizga yangi izoh qoshildi", cancellationToken);
    }

    public async ValueTask DeleteCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default)
    {
        var commentEntity = new Entities.Comment();

        var role = await userService.GetUserRoleAsync(userId);

        if (role == "Admin" && role == "Moderator")
            commentEntity = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);

        else if(role == "Author")
            commentEntity = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId, cancellationToken);
        
        if (commentEntity is null)
            throw new KeyNotFoundException($"Comment with id {commentId} not found.");

        dbContext.Comments.Remove(commentEntity);
        await dbContext.SaveChangesAsync(cancellationToken);

        if(commentEntity.UserId != userId)
        {
            string notificationMessage = "Siz yozgan comment ochirildi";
            
            await hubContext.Clients.User(userId.ToString()).SendAsync(notificationMessage, cancellationToken);
        }
    }
}
