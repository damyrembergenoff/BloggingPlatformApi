namespace BloggingPLatformApi.Abstractions;

public interface IMediaAttachmentService
{
    ValueTask UploadMediaAsync(Guid userId, Guid postId, Dtos.CreateMedia media, CancellationToken cancellationToken = default);
    ValueTask DeleteMediaAsync(Guid userId, Guid mediaId, CancellationToken cancellationToken = default);
}
