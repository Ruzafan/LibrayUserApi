using MongoDB.Driver;
using UsersApi.Entities;
using UsersApi.Repositories;

namespace UsersApi.Features.GetUser.V1;

public class Handler(IRepository<User> userRepository, IRepository<FriendRequest> friendRequestRepository)
{
    public async Task<Response?> Handle(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.Get(userId, cancellationToken);
        if (user == null) return null;
        var filterFriendRequest = Builders<FriendRequest>.Filter.Or(
            Builders<FriendRequest>.Filter.Eq(u => u.RequestedId, userId),
            Builders<FriendRequest>.Filter.Eq(u => u.RequestorId, userId));
        var friendRequests = await friendRequestRepository.QueryItems(filterFriendRequest, cancellationToken);
        return user.ToResponse(friendRequests.Where(q=>q.Status != RequestStatus.Declined).ToList());
    }
}