using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Data;
using BloggingPLatformApi.Dtos;
using BloggingPLatformApi.Mappers;
using BloggingPLatformApi.Services;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatformApi.Services;

public class UserRegisteryService(AppDbContext dbContext) : IUserService
{
    public async ValueTask<BloggingPLatformApi.Entities.User> AddUserAsync(CreateOrLoginUser user, CancellationToken cancellationToken = default)
    {
        var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == user.Username);

        if(existingUser is not null)
            throw new Exception($"User with username {user.Username} already exists");

        var entityEntry = dbContext.Users.Add(user.ToEntity());
        await dbContext.SaveChangesAsync(cancellationToken);

        return entityEntry.Entity;
    }

    public async ValueTask<BloggingPLatformApi.Entities.User> LoginUserAsync(CreateOrLoginUser loginUser, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == loginUser.Username, cancellationToken);

        if(user is null)
            throw new Exception("User not found");

        if(!loginUser.PasswordHash.VerifyPassword(user.PasswordHash!))
            throw new Exception("Invalid password");

        return user;
    }

    public async ValueTask<string> GetUserRoleAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if(user!.Role == "Author")
            return "Author";

        if(user!.Role == "Admin")
            return "Admin";

        return "Moderator";
    }

}