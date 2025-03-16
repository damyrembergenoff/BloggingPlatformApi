using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Data;
using BloggingPLatformApi.Dtos;
using BloggingPLatformApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace BloggingPLatformApi.Services;

public class PostService(AppDbContext dbContext, IUserService userService) : IPostService
{
    public async ValueTask CreatePostAsync(Guid userId, CreatePost post, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Posts.AddAsync(post.ToEntity(userId), cancellationToken);

        if(post.TagIds is not null && post.TagIds.Count() > 0)
            foreach(var id in post.TagIds)
                await dbContext.PostTags.AddAsync(new Entities.PostTag { PostId = entry.Entity.Id, TagId = id });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeletePostAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
    {
        var post = new Entities.Post();

        if(await userService.GetUserRoleAsync(userId) == "Admin")
            post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);

        else if(await userService.GetUserRoleAsync(userId) == "Author")
            post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId && x.UserId == userId, cancellationToken);

        if(post is not null)
        {
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        else
            throw new KeyNotFoundException($"Post with {postId} not found");
    }

    public async ValueTask<Post> GetPostAsync(Guid userId, Guid postId, CancellationToken cancellationToken = default)
    {
        var post = await dbContext.Posts.AsNoTracking().AsQueryable()
            .Include(x => x.Category)
            .Include(x => x.Comments)
            .Include(x => x.Likes)
            .Include(x => x.MediaAttachments)
            .Include(x => x.User)
            .Include(x => x.PostTags)
            .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);

        if(post is null)
            throw new KeyNotFoundException($"Post with {postId} not found");

        if(await userService.GetUserRoleAsync(userId) == "Admin")
            return post.ToDto();

        if(post.UserId == userId)
            return post.ToDto();
        
        throw new Exception("You are not authorithazed to see this post");
    }

    public async ValueTask<IEnumerable<Post>> GetPostsAsync(Guid userId, PostFilter postFilter, CancellationToken cancellationToken = default)
    {
        var postQuery = dbContext.Posts.AsNoTracking().AsQueryable();
        
        if(await userService.GetUserRoleAsync(userId) == "Author")
            postQuery = postQuery.Where(x => x.UserId == userId);

        if(postFilter.Status is not null)
            postQuery = postQuery.Where(post => post.Status == postFilter.Status);

        postQuery = postQuery.Include(x => x.Category);
        postQuery = postQuery.Include(x => x.PostTags);
        postQuery = postQuery.Include(x => x.MediaAttachments);
        postQuery = postQuery.Include(x => x.Comments);
        postQuery = postQuery.Include(x => x.Likes);
        postQuery = postQuery.Include(x => x.User);


        if(postFilter.CategoryId is not null)
            postQuery = postQuery.Where(post => post.CategoryId == postFilter.CategoryId);

        if(postFilter.TagId is not null)
            postQuery = postQuery.Where(x => x.PostTags != null && x.PostTags.Any(x => x.TagId == postFilter.TagId));

        if(!string.IsNullOrWhiteSpace(postFilter.Search))
        {
            var searchPattern = $"%{postFilter.Search.Trim()}%";
            postQuery = postQuery.Where(x => EF.Functions.ILike(x.Title!, searchPattern) 
            || EF.Functions.ILike(x.Content!, searchPattern));
        }

        var posts = await postQuery.ToListAsync(cancellationToken);
        return posts.Select(x => x.ToDto());    
    }

    public async ValueTask UpdatePostAsync(Guid userId, Guid postId, UpdatePost updatePost, CancellationToken cancellationToken = default)
    {
        var post = await dbContext.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == postId && x.UserId == userId, cancellationToken);

        if(post is null)
            throw new KeyNotFoundException($"post with {postId} not found");

        var updatedPost = updatePost.ToEntity(userId);
        updatedPost.Id = postId;

        if(updatePost.TagIds is not null && updatePost.TagIds.Count() > 0)
            foreach(var id in updatePost.TagIds)
                await dbContext.PostTags.AddAsync(new Entities.PostTag { PostId = postId, TagId = id });

        dbContext.Posts.Update(updatedPost);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
