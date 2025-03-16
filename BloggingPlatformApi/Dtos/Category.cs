namespace BloggingPLatformApi.Dtos;

public class Category : DtoBase
{
    public string Name { get; set; } = default!;

    public ICollection<PostListItem>? Posts { get; set; }
}

public class CategoryListItem : DtoBase
{
    public string Name { get; set; } = default!; 
}