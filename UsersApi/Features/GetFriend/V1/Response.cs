namespace UsersApi.Features.GetFriend.V1;

public class Response
{
    public required string UserName { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Image { get; set; }
}