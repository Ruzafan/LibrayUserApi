namespace UsersApi.Features.Login.V1;

public class Request
{
    public required string Username { get; set; }
    
    public required string Password { get; set; }
}