using MongoDB.Driver;
using UsersApi.Entities;
using UsersApi.Repositories;

namespace UsersApi.Features.GetFriend.V1;

public class Handler(IRepository<User> userRepository, IRepository<FriendRequest> friendRequestRepository)
{
    public async Task<Response?> Handle(string userId, string friend, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Or(
            Builders<User>.Filter.Eq(u => u.Id, userId),
            Builders<User>.Filter.Eq(u => u.Id, friend));
        var users = await userRepository.QueryItems(filter, cancellationToken);
        if (users.Count < 2) return null;
        var friendUser = users.FirstOrDefault(q => q.Username == friend);
        return friendUser?.ToResponse();

    }
}