using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPLatformApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async ValueTask<IActionResult> CreateOrLoginUser(Dtos.CreateOrLoginUser user, CancellationToken cancellationToken)
    {
        try
        {
            var createdUser = await userService.AddUserAsync(user, cancellationToken);

            return Ok(createdUser.GenerateToken(configuration));
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async ValueTask<IActionResult> LoginUser(Dtos.CreateOrLoginUser loginUser, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userService.LoginUserAsync(loginUser, cancellationToken);

            return Ok(user.GenerateToken(configuration));
        }
        catch(Exception ex)
        {
            return BadRequest(ex);
        }
    }
}