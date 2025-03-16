namespace BloggingPLatformApi.Entities;

public class MediaAttachment : EntityBase
{
    public string FileUrl { get; set; } = default!;
    public string FileType { get; set; } = default!;
    public Guid PostId { get; set; }

    public Post Post { get; set; } = default!;
}
