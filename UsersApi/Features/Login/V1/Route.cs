using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace UsersApi.Features.Login.V1;

public static class Route
{
    public static void MapLoginEndpoint(this WebApplication app)
    {
        app.MapPost("/user/login/v1", async (Request request, CancellationToken cancellationToken, Handler handler) =>
            {
                var response = await handler.Handle(request, cancellationToken);
                return response == null ? Results.NotFound() : Results.Ok(response);
            })
            .WithName("Login");
    }
}