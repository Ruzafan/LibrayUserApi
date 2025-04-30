using UsersApi.Entities;

namespace UsersApi.Features.GetUser.V1;

public class Response
{
    public required string UserName { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Image { get; set; }
    public DateTime Create { get; set; }
    public List<Friend> Friends { get; set; }
}

public class Friend
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public RequestStatus Status { get; set; }
    public string Image { get; set; }
}