using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Dtos;

namespace Ilmhub.Lms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController(ICommentService commentService, IAuthenticationStateService authenticationStateService) : ControllerBase
{
    [HttpPost("{postId}/comment")]
    [Authorize(Roles = "Author")]
    public async Task<IActionResult> AddComment(Guid postId, [FromBody] CreateComment comment, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(authenticationStateService.UserId))
                return Unauthorized();

            var userId = Guid.Parse(authenticationStateService.UserId!);
            await commentService.AddCommentAsync(userId, postId, comment, cancellationToken);
            return Ok();
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(Guid commentId, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(authenticationStateService.UserId))
            return Unauthorized();

            var userId = Guid.Parse(authenticationStateService.UserId!);
            await commentService.DeleteCommentAsync(userId, commentId, cancellationToken);
            return NoContent();
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
