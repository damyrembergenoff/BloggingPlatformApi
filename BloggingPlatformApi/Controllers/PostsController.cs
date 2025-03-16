using BloggingPLatformApi.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPLatformApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController(IPostService postService, IAuthenticationStateService authenticationStateService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Author")]
    public async Task<IActionResult> CreatePost(Dtos.CreatePost post, CancellationToken cancellationToken)
    {
        await postService.CreatePostAsync(Guid.Parse(authenticationStateService.UserId!), post, cancellationToken);
        return Created();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Author")]
    public async Task<IActionResult> GetPost(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var post = await postService.GetPostAsync(Guid.Parse(authenticationStateService.UserId!), id, cancellationToken);
            return Ok(post);
        }
        catch(KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(Exception ex)
        {
            return Unauthorized (ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Author")]
    public async Task<IActionResult> GetPosts([FromQuery] Dtos.PostFilter filter, CancellationToken cancellationToken)
        => Ok(await postService.GetPostsAsync(Guid.Parse(authenticationStateService.UserId!), filter, cancellationToken));

    [HttpPut]
    [Authorize(Roles = "Author")]
    public async Task<IActionResult> UpdatePost(Guid id, Dtos.UpdatePost post, CancellationToken cancellationToken)
    {
        await postService.UpdatePostAsync(Guid.Parse(authenticationStateService.UserId!), id, post, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Author")]
    public async Task<IActionResult> DeletePost(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await postService.DeletePostAsync(Guid.Parse(authenticationStateService.UserId!), id, cancellationToken);
            return NoContent();
        }
        catch(KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}