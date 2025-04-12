using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
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
            Name = request.Name,
            Surname = request.Surname,
            Image = await UploadImage(request.Image),
            Created = DateTime.UtcNow,
        }, cancellationToken);

        return new Response();
    }
    private async Task<string> UploadImage(IFormFile file)
    {
        var storageConnectionString = config["AzureBlob_ConnectionString"];
        var containerName = "user_images";

        var blobServiceClient = new BlobServiceClient(storageConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream);
        }

        return $"https://readrstorage.blob.core.windows.net/images/{fileName}";
            
    }

}