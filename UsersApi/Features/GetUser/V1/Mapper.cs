using UsersApi.Entities;

namespace UsersApi.Features.GetUser.V1;

public static class Mapper
{
    public static Response ToResponse(this User user)
    {
        return new Response()
        {
            Name = user.Name,
            Image = user.Image,
            
        };
    }
}