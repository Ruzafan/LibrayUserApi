using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver.Linq;

namespace UsersApi.Features.GetUser.V1;

public static class Route
{
    public static void MapGetUserEndpoint(this WebApplication app)
    {
        app.MapGet("/user/v1", async (HttpContext context, CancellationToken cancellationToken, Handler handler) =>
            {
                var token = await context.GetTokenAsync("access_token");
                var tokenRead = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userId = tokenRead.Claims.First(q=>q.Type == ClaimTypes.Name).Value;
                var response = await handler.Handle(userId, cancellationToken);
                return response == null ? Results.NotFound() : Results.Ok(response);
            })
            .WithName("GetUser")
            .RequireAuthorization();
    }
    
    
}