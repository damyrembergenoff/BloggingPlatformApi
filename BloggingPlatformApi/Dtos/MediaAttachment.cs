namespace BloggingPLatformApi.Dtos;

public class MediaAttachment : DtoBase
{
    public string FileUrl { get; set; } = default!;
    public string FileType { get; set; } = default!;
    public Guid PostId { get; set; }

    public PostListItem Post { get; set; } = default!;
}

public class MediaAttachmentListItem : DtoBase
{
    public string FileUrl { get; set; } = default!;
    public string FileType { get; set; } = default!;
    public Guid PostId { get; set; }
}

public class CreateMedia
{
    public string FileUrl { get; set; } = default!;
    public string FileType { get; set; } = default!;
}