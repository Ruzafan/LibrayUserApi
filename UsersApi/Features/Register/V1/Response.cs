namespace UsersApi.Features.Register.V1;

public class Response
{
    public bool UserAlreadyExist;
    public Response SetUserAlreadyExist()
    {
        this. UserAlreadyExist = true;
        return this;
    }
}