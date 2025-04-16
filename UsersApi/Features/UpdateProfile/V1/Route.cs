using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace UsersApi.Features.UpdateProfile.V1;

public static class Route
{
    public static void MapUpdateProfileEndpoint(this WebApplication app)
    {
        app.MapPatch("/user/v1/updateprofile", async (HttpContext context, HttpRequest request, CancellationToken cancellationToken, Handler handler) =>
            {
                var form = await request.ReadFormAsync(cancellationToken);
                var file = form.Files["image"];
                if (file == null || file.Length == 0)
                    return Results.BadRequest("Image is required.");
                
                var token = await context.GetTokenAsync("access_token");
                var tokenRead = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userId = tokenRead.Claims.First(q=>q.Type == ClaimTypes.Name).Value;
                var response = await handler.Handle(userId,file , cancellationToken);
                return response ? Results.Ok() : Results.NotFound();
            })
            .WithName("UpdateProfile")
            .RequireAuthorization();
    }
}