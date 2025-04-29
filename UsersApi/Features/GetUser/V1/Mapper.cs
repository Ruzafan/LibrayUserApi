using UsersApi.Entities;

namespace UsersApi.Features.GetUser.V1;

public static class Mapper
{
    public static Response ToResponse(this User user, List<FriendRequest> friendsRequests)
    {
        return new Response()
        {
            UserName = user.Username,
            Name = user.Name,
            Surname = user.Surname,
            Image = user.Image,
            Create = user.Created,
            //Friends = friendsRequests.ToFriends()
        };
    }

    // public static List<Friend> ToFriends(this User user, List<FriendRequest> friendsRequests)
    // {
    //     friendsRequests.Select(q=> new Friend()
    //     {
    //         UserName = q.,
    //     })
    // }
}