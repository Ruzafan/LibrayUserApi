using MongoDB.Driver;
using UsersApi.Entities;
using UsersApi.Repositories;

namespace UsersApi.Features.AddFriend.V1;

public class Handler(IRepository<User> userRepository, IRepository<FriendRequest> friendRequestRepository)
{
    public async Task<bool> Handle(string userId, string friend, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Or(
            Builders<User>.Filter.Eq(u => u.Id, userId),
            Builders<User>.Filter.Eq(u => u.Username, friend));
        var users = await userRepository.QueryItems(filter, cancellationToken);
        if (users.Count < 2) return false;
        var user = users.FirstOrDefault(q=>q.Id == userId);
        var friendUser = users.FirstOrDefault(q => q.Username == friend);
        if (user != null && friendUser != null)
        {
            var filterFriendRequest = Builders<FriendRequest>.Filter.Or(
                Builders<FriendRequest>.Filter.In(u => u.RequestedId, users.Select(q=>q.Id)),
                Builders<FriendRequest>.Filter.In(u => u.RequestorId, users.Select(q=>q.Id)));
            var friendRequests = await friendRequestRepository.QueryItems(filterFriendRequest, cancellationToken);
            if (friendRequests == null || friendRequests.Count == 0) 
            {
                await friendRequestRepository.Add(new FriendRequest()
                {
                    RequestedId = friendUser.Id,
                    RequestorId = userId,
                    Created = DateTime.Now,
                    Status = RequestStatus.Requested
                }, cancellationToken);
                return true;
            }
            var friendRequest = friendRequests.FirstOrDefault();
            if (friendRequest.RequestedId == userId && friendRequest.RequestorId == friendUser.Id &&
                friendRequest.Status == RequestStatus.Requested)
            {
                var update =Builders<FriendRequest>.Update.Set(f => f.Status, RequestStatus.Accepted);
                await friendRequestRepository.Update(friendRequest.Id, update, cancellationToken);
            }

            return true;
        }

        return false;
    }
}