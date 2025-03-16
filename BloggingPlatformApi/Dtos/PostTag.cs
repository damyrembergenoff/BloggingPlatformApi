namespace BloggingPLatformApi.Dtos;

public class PostTag : DtoBase
{
    public Guid PostId { get; set; }
    public Guid TagId { get; set; }

    public PostListItem Post { get; set; } = default!;
    public TagListItem Tag { get; set; } = default!;
}

public class PostTagListItem : DtoBase
{
    public Guid PostId { get; set; }
    public Guid TagId { get; set; }
}