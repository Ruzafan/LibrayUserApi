
namespace UsersApi.Entities;

public class User : Mongeable
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public byte[] Password { get; set; }
    public List<string> Friends { get; set; }

}