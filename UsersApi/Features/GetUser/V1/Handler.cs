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
        friendRequests = friendRequests.Where(q => q.Status != RequestStatus.Declined).ToList();
        var friendsIds = friendRequests.Where(q => q.RequestedId != userId).Select(q => q.RequestedId).ToList();
        friendsIds.AddRange( friendRequests.Where(q=>q.RequestorId != userId).Select(q=>q.RequestorId).ToList());
        
        var friendUserFilter = Builders<User>.Filter.In(u => u.Id, friendsIds);
        var users = await userRepository.QueryItems(friendUserFilter, cancellationToken);
        return user.ToResponse(friendRequests,users);
    }
}