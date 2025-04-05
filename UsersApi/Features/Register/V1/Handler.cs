using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UsersApi.Entities;
using UsersApi.Extensions;
using UsersApi.Repositories;

namespace UsersApi.Features.Register.V1;

public class Handler (IRepository<User> userRepository, IConfiguration config)
{
    public async Task<Response?> Handle(Request request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.QueryItems(Builders<User>.Filter.Eq(q => q.Username, request.Username), cancellationToken);
        if (user.Any()) 
            return new Response()
                .SetUserAlreadyExist();

        await userRepository.Add(new User()
        {
            Username = request.Username,
            Password = request.Password.CalculateSha256(),
        }, cancellationToken);

        return new Response();
    }

}