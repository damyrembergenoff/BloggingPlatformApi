using System;
using System.Threading;
using System.Threading.Tasks;
using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Data;
using BloggingPLatformApi.Entities;
using BloggingPLatformApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Ilmhub.Lms.Api.Services;

public class LikeService(AppDbContext dbContext,
    INotificationService notificationService,
    IHubContext<NotificationHub> hubContext) : ILikeService
{
    public async ValueTask LikePostAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
    {
        var existingLike = await dbContext.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId, cancellationToken);
        
        if (existingLike is not null)
            throw new Exception("Post already liked.");

        var likedPost = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        if(likedPost is null)
            throw new Exception($"Post with id {postId} not found");

        var like = new Like
        {
            PostId = postId,
            UserId = userId
        };

        await dbContext.Likes.AddAsync(like, cancellationToken);
        
        if(like.UserId != likedPost.UserId)
        {
            string notificationMessage = "Sizning postingizga like bosildi";
            
            await hubContext.Clients.User(userId.ToString()).SendAsync(notificationMessage, cancellationToken);
            await notificationService.SendNotificationAsync(likedPost.UserId, notificationMessage, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask UnlikePostAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
    {
        var existingLike = await dbContext.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId, cancellationToken);
        
        if (existingLike is null)
            throw new Exception("Post is not liked yet.");

        dbContext.Likes.Remove(existingLike);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
