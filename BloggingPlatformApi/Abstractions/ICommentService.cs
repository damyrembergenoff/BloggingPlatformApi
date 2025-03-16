namespace BloggingPLatformApi.Abstractions;

public interface ICommentService
{
    ValueTask AddCommentAsync(Guid userId, Guid postId, Dtos.CreateComment comment, CancellationToken cancellationToken = default);
    ValueTask DeleteCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default);
}
