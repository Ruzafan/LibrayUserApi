using UsersApi.Entities;
using UsersApi.Repositories;

namespace UsersApi.Features.GetUser.V1;

public class Handler(IRepository<User> userRepository)
{
    public async Task<Response?> Handle(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.Get(userId, cancellationToken);
        if (user == null) return null;
        return user.ToResponse();
    }
}