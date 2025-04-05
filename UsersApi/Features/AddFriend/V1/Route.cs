using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace UsersApi.Features.AddFriend.V1;

public static class Route
{
    public static void MapAddFriendEndpoint(this WebApplication app)
    {
        app.MapPatch("/user/v1/addfriend", async (HttpContext context, Request request, CancellationToken cancellationToken, Handler handler) =>
            {
                var token = await context.GetTokenAsync("access_token");
                var tokenRead = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userId = tokenRead.Claims.First(q=>q.Type == ClaimTypes.Name).Value;
                var response = await handler.Handle(userId, request.FriendId, cancellationToken);
                return response ? Results.Ok() : Results.NotFound();
            })
            .WithName("AddFriend")
            .RequireAuthorization();
    }
}