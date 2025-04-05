namespace UsersApi.Features.Register.V1;

public class Request
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Image {get; set;}
}