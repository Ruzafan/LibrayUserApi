using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using UsersApi.Entities;
using UsersApi.Features.AddFriend.V1;
using UsersApi.Features.GetFriend.V1;
using UsersApi.Features.GetUser.V1;
using UsersApi.Features.Login.V1;
using UsersApi.Features.Register.V1;
using UsersApi.Features.UpdateProfile.V1;

namespace UsersApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetValue<string>("MONGODB_URI"));
        services.AddSingleton(mongoClient);
        return services;
    }

    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddSingleton<Features.AddFriend.V1.Handler>();
        services.AddSingleton<Features.Login.V1.Handler>();
        services.AddSingleton<Features.GetUser.V1.Handler>();
        services.AddSingleton<Features.Register.V1.Handler>();
        services.AddSingleton<Features.UpdateProfile.V1.Handler>();
        services.AddSingleton<Features.GetFriend.V1.Handler>();
        
        services.AddSingleton<Repositories.IRepository<User>, Repositories.Repository<User>>();
        services.AddSingleton<Repositories.IRepository<FriendRequest>, Repositories.Repository<FriendRequest>>();
        return services;
    }
    
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapLoginEndpoint();
        app.MapGetUserEndpoint();
        app.MapRegisterEndpoint();
        app.MapAddFriendEndpoint();
        app.MapUpdateProfileEndpoint();
        app.MapGetFriendEndpoint();
    }
    
    public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "your_issuer",
                ValidAudience = "your_audience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("SecretKey","")))
            };
        });
        return services;
    }
}