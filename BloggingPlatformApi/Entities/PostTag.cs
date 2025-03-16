namespace BloggingPLatformApi.Entities;

public class PostTag : EntityBase
{
    public Guid PostId { get; set; }
    public Guid TagId { get; set; }

    public Post Post { get; set; } = default!;
    public Tag Tag { get; set; } = default!;
}
