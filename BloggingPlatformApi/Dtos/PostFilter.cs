namespace BloggingPLatformApi.Dtos;

public class PostFilter
{
    public string? Status { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? TagId { get; set; }
    public string? Search { get; set; }

    public string ToQueryString()
        => $"""
            ?status={Status}
            &categoryId={CategoryId}
            &tagId={TagId}
            &search={Search}
            """;
}