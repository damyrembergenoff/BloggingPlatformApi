namespace BloggingPLatformApi.Abstractions;

public interface ILikeService
{
    ValueTask LikePostAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
    ValueTask UnlikePostAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default);
}
