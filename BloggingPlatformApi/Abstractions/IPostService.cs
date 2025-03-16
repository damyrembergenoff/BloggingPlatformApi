namespace BloggingPLatformApi.Abstractions;

public interface IPostService
{
    ValueTask CreatePostAsync(Guid userId, Dtos.CreatePost post, CancellationToken cancellationToken = default);
    ValueTask UpdatePostAsync(Guid userId, Guid id, Dtos.UpdatePost post, CancellationToken cancellationToken = default);
    ValueTask DeletePostAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<Dtos.Post>> GetPostsAsync(Guid userId, Dtos.PostFilter postFilter, CancellationToken cancellationToken = default);
    ValueTask<Dtos.Post> GetPostAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
