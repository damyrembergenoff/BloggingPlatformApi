using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Dtos;

namespace BloggingPLatformApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediaAttachmentsController(IAuthenticationStateService authStateService, IMediaAttachmentService mediaAttachmentService) : ControllerBase
{
    [HttpPost("{postId}")]
    [Authorize(Roles = "Author")]
    public async Task<IActionResult> UploadMedia(Guid postId, [FromBody] CreateMedia media, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(authStateService.UserId))
            return Unauthorized();
        
        var userId = Guid.Parse(authStateService.UserId!);
        
        try
        {
            await mediaAttachmentService.UploadMediaAsync(userId, postId, media, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{postId}")]
    [Authorize(Roles = "Author,Admin,Moderator")]
    public async Task<IActionResult> DeleteMedia(Guid mediaId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(authStateService.UserId))
            return Unauthorized();
        
        var userId = Guid.Parse(authStateService.UserId!);
        
        try
        {
            await mediaAttachmentService.DeleteMediaAsync(userId, mediaId, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
