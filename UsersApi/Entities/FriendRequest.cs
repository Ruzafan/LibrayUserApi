namespace UsersApi.Entities;

public class FriendRequest : Mongeable
{
    public string RequestorId { get; set; }
    public string RequestedId { get; set; }
    public RequestStatus Status { get; set; }
}