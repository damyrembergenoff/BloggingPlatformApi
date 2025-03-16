namespace BloggingPLatformApi.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = default!;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
