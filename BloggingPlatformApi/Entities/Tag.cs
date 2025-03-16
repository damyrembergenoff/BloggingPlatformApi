namespace BloggingPLatformApi.Entities;

public class Tag : EntityBase
{
    public string Name { get; set; } = default!;

    public ICollection<PostTag> PostTags { get; set; } = [];
}
