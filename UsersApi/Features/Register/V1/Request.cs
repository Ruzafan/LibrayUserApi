using Microsoft.AspNetCore.Http;

namespace UsersApi.Features.Register.V1;

public class Request
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public IFormFile Image {get; set;}
}