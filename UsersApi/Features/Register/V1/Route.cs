using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace UsersApi.Features.Register.V1;

public static class Route
{
    public static void MapRegisterEndpoint(this WebApplication app)
    {
        app.MapPost("/user/register/v1", async (HttpRequest request, CancellationToken cancellationToken, Handler handler) =>
            {
                var form = await request.ReadFormAsync(cancellationToken);
                var handlerRequest = new Request
                {
                    Username = form["username"],
                    Password = form["password"],
                    Name = form["name"],
                    Surname = form["surname"],
                };

                var file = form.Files["image"];
                if (file == null || file.Length == 0)
                    return Results.BadRequest("Image is required.");
                handlerRequest.Image = file;
                var response = await handler.Handle(handlerRequest, cancellationToken);
                return response.UserAlreadyExist ? Results.Problem() : Results.Created();
            })
            .WithName("Register");
    }
}