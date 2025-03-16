namespace BloggingPLatformApi.Dtos;

public class Tag : DtoBase
{
    public string Name { get; set; } = default!;

    public ICollection<PostTagListItem> PostTags { get; set; } = [];
}

public class TagListItem : DtoBase
{
    public string Name { get; set; } = default!;
}