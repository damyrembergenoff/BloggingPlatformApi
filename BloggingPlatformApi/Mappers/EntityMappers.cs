using BloggingPLatformApi.Services;

namespace BloggingPLatformApi.Mappers;

public static class EntityMappers
{
    public static Dtos.Post ToDto(this Entities.Post post)
        => new()
        {
            Id = post.Id,
            Title = post.Title!,
            Content = post.Content!,
            Status = post.Status!,
            CreatedAt = post.CreatedAt,
            PublishedAt = post.PublishedAt,
            UserId = post.UserId,
            CategoryId = post.CategoryId,

            User = post.User?.ToListItemDto(),
            Category = post.Category?.ToListItemDto(),

            Comments = post.Comments?.Select(x => x.ToListItemDto()).ToList() ?? [],
            Likes = post.Likes?.Select(x => x.ToListItemDto()).ToList() ?? [],
            MediaAttachments = post.MediaAttachments?.Select(x => x.ToListItemDto()).ToList() ?? [],
            PostTags = post.PostTags?.Select(x => x.ToListItemDto()).ToList() ?? []
        };

    public static Entities.Post ToEntity(this Dtos.CreatePost post, Guid userId)
        => new()
        {
            Id = Guid.NewGuid(),
            Title = post.Title,
            Content = post.Content,
            Status = post.Status,
            CategoryId = post.CategoryId,
            UserId = userId,
        };

    public static Entities.Post ToEntity(this Dtos.UpdatePost post, Guid userId)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = post.Title,
            Content = post.Content,
            Status = post.Status,
            CategoryId = post.CategoryId,
            PublishedAt = post.PublishedAt
        };

    public static Dtos.PostListItem ToListItemDto(this Entities.Post post)
        => new()
        {
            Id = post.Id,
            Title = post.Title!,
            Content = post.Content!,
            Status = post.Status!,
            CreatedAt = post.CreatedAt,
            PublishedAt = post.PublishedAt,
            UserId = post.UserId,
            CategoryId = post.CategoryId
        };

    public static Dtos.Comment ToDto(this Entities.Comment comment)
        => new()
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            PostId = comment.PostId,
            UserId = comment.UserId,

            Post = comment.Post?.ToListItemDto()!,
            User = comment.User?.ToListItemDto()!
        };

    public static Dtos.CommentListItem ToListItemDto(this Entities.Comment comment)
        => new()
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            PostId = comment.PostId,
            UserId = comment.UserId
        };

    public static Entities.Comment ToEntity(this Dtos.CreateComment comment, Guid userId)
        => new()
        {
            UserId = userId,
            Content = comment.Content,
        };

    public static Dtos.User ToDto(this Entities.User user)
        => new()
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
        };

    public static Dtos.UserListItem ToListItemDto(this Entities.User user)
        => new()
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };

    public static Dtos.Category ToDto(this Entities.Category category)
        => new()
        {
            Id = category.Id,
            Name = category.Name,
            Posts = category.Posts?.Select(x => x.ToListItemDto()).ToList()
        };

    public static Dtos.CategoryListItem ToListItemDto(this Entities.Category category)
        => new()
        {
            Id = category.Id,
            Name = category.Name,
        };

    public static Dtos.Like ToDto(this Entities.Like like)
        => new()
        {
            Id = like.Id,
            PostId = like.PostId,
            UserId = like.UserId,

            Post = like.Post?.ToListItemDto()!,
            User = like.User?.ToListItemDto()!
        };

    public static Dtos.LikeListItem ToListItemDto(this Entities.Like like)
        => new()
        {
            Id = like.Id,
            PostId = like.PostId,
            UserId = like.UserId,
        };

    public static Dtos.MediaAttachment ToDto(this Entities.MediaAttachment media)
        => new()
        {
            Id = media.Id,
            FileUrl = media.FileUrl,
            FileType = media.FileType,
            PostId = media.PostId,

            Post = media.Post?.ToListItemDto()!
        };

    public static Dtos.MediaAttachmentListItem ToListItemDto(this Entities.MediaAttachment media)
        => new()
        {
            Id = media.Id,
            FileUrl = media.FileUrl,
            FileType = media.FileType,
            PostId = media.PostId
        };

    public static Entities.MediaAttachment ToEntity(this Dtos.CreateMedia media)
        => new()
        {
            FileUrl = media.FileUrl,
            FileType = media.FileType,
        };

    public static Dtos.PostTag ToDto(this Entities.PostTag postTag)
        => new()
        {
            Id = postTag.Id,
            PostId = postTag.PostId,
            TagId = postTag.TagId,

            Post = postTag.Post?.ToListItemDto()!,
            Tag = postTag.Tag?.ToListItemDto()!
        };

    public static Dtos.PostTagListItem ToListItemDto(this Entities.PostTag postTag)
        => new()
        {
            Id = postTag.Id,
            PostId = postTag.PostId,
            TagId = postTag.TagId
        };

    public static Dtos.Notification ToDto(this Entities.Notification notification)
        => new()
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,

            User = notification.User?.ToListItemDto()!
        };
    
    public static Dtos.NotificationListItem ToListItemDto(this Entities.Notification notification)
        => new()
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };

    public static Entities.User ToEntity(this Dtos.CreateOrLoginUser user)
        => new()
        {
            Username = user.Username,
            PasswordHash = user.PasswordHash.HashPassword(),
            Role = "Author"
        };

    public static Dtos.Tag ToDto(this Entities.Tag tag)
        => new()
        {
            Id = tag.Id,
            Name = tag.Name,

            PostTags = tag.PostTags?.Select(x => x.ToListItemDto()).ToList() ?? []
        };
    
    public static Dtos.TagListItem ToListItemDto(this Entities.Tag tag)
        => new()
        {
            Id = tag.Id,
            Name = tag.Name
        };
}