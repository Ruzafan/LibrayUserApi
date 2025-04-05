using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace UsersApi.Features.Register.V1;

public static class Route
{
    public static void MapRegisterEndpoint(this WebApplication app)
    {
        app.MapPost("/user/register/v1", async (Request request, CancellationToken cancellationToken, Handler handler) =>
            {
                var response = await handler.Handle(request, cancellationToken);
                return response.UserAlreadyExist ? Results.Problem() : Results.Created();
            })
            .WithName("Register");
    }
}