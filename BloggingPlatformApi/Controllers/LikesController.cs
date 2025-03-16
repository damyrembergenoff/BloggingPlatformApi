using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloggingPLatformApi.Abstractions;

namespace Ilmhub.Lms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Author")]
public class LikesController(ILikeService likeService, IAuthenticationStateService authenticationStateService) : ControllerBase
{

    [HttpPost("{postId}")]
    public async Task<IActionResult> LikePost(Guid postId, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(authenticationStateService.UserId))
            return Unauthorized();

            var userId = Guid.Parse(authenticationStateService.UserId!);
            await likeService.LikePostAsync(userId, postId, cancellationToken);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> UnlikePost(Guid postId, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(authenticationStateService.UserId))
            return Unauthorized();

            var userId = Guid.Parse(authenticationStateService.UserId!);
            await likeService.UnlikePostAsync(userId, postId, cancellationToken);
            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
