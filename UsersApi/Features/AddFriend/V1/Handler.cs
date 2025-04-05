using UsersApi.Entities;
using UsersApi.Repositories;

namespace UsersApi.Features.AddFriend.V1;

public class Handler(IRepository<User> userRepository)
{
    public async Task<bool> Handle(string userId, string friend, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.Get(userId, cancellationToken);
        if (user != null)
        {
            await userRepository.AddFriend(user.Id, friend, cancellationToken);
            return true;
        }

        return false;
    }
}