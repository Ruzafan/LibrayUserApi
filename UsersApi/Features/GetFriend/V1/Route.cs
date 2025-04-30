using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsersApi.Features.GetFriend.V1;

public static class Route
{
    public static void MapGetFriendEndpoint(this WebApplication app)
    {
        app.MapGet("/user/v1/friend", async ([FromQuery] string friendId,HttpContext context, CancellationToken cancellationToken, Handler handler) =>
            {
                var token = await context.GetTokenAsync("access_token");
                var tokenRead = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userId = tokenRead.Claims.First(q=>q.Type == ClaimTypes.Name).Value;
                var response = await handler.Handle(userId, friendId, cancellationToken);
                return response != null ? Results.Ok(response) : Results.NotFound();
            })
            .WithName("GetFriend")
            .RequireAuthorization();
    }
}