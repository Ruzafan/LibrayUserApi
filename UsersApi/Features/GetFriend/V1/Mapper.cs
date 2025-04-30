using UsersApi.Entities;

namespace UsersApi.Features.GetFriend.V1;

public static class Mapper
{
    public static Response ToResponse(this User user)
    {
        return new Response()
        {
            UserName = user.Username,
            Name = user.Name,
            Surname = user.Surname,
            Image = user.Image,
        };
    }

   
}