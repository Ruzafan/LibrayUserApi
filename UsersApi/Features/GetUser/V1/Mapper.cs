using UsersApi.Entities;

namespace UsersApi.Features.GetUser.V1;

public static class Mapper
{
    public static Response ToResponse(this User user, List<FriendRequest> friendsRequests, List<User> users)
    {
        return new Response()
        {
            UserName = user.Username,
            Name = user.Name,
            Surname = user.Surname,
            Image = user.Image,
            Create = user.Created,
            Friends = friendsRequests.ToFriends(user, users)
        };
    }

    private static List<Friend> ToFriends(this List<FriendRequest> friendsRequests, User user, List<User> friends)
    {
        var result = new List<Friend>();
        friendsRequests.ForEach(q =>
        {
            var currentFriend = friends.First(f => f.Id == (q.RequestedId == user.Id ? q.RequestorId : q.RequestedId));
            result.Add(new Friend()
            {
                Id = currentFriend.Id,
                UserName = currentFriend.Username,
                Image = currentFriend.Image,
                Status = q.Status,
            });
        });
        return result;
    }
}