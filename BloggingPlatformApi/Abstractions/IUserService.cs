namespace BloggingPLatformApi.Abstractions;

public interface IUserService
{
    ValueTask<Entities.User> AddUserAsync(Dtos.CreateOrLoginUser user, CancellationToken cancellationToken = default);
    ValueTask<Entities.User> LoginUserAsync(Dtos.CreateOrLoginUser user, CancellationToken cancellationToken);
    ValueTask<string> GetUserRoleAsync(Guid userId, CancellationToken cancellationToken = default);
}