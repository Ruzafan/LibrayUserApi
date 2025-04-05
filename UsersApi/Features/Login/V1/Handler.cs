using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using UsersApi.Entities;
using UsersApi.Extensions;
using UsersApi.Features.GetUser.V1;
using UsersApi.Repositories;
namespace UsersApi.Features.Login.V1;

public class Handler (IRepository<User> userRepository, IConfiguration config)
{
    public async Task<Response?> Handle(Request request, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter;
        var queryFilter = filter.And(filter.Eq(q=>q.Username, request.Username),
            filter.Eq(q=>q.Password, request.Password.CalculateSha256()));
        var user = await userRepository.QueryItems(queryFilter, cancellationToken);
        if (user == null || !user.Any()) return null;
        return new Response()
        {
            Token = GenerateJwtToken(user.First().Id)
        };
    }
    
    private string GenerateJwtToken(string id)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("SecretKey","")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your_issuer",
            audience: "your_audience",
            claims: new List<Claim>()
            {
                new(ClaimTypes.Name, id),
            },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}