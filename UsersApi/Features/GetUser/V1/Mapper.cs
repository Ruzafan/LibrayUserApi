using UsersApi.Entities;

namespace UsersApi.Features.GetUser.V1;

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
            Create = user.Created,
            ProfileId = user.Id,
        };
    }
}