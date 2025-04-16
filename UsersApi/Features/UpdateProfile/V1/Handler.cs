using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UsersApi.Entities;
using UsersApi.Repositories;

namespace UsersApi.Features.UpdateProfile.V1;

public class Handler(IRepository<User> userRepository, IConfiguration config)
{
    public async Task<bool> Handle(string userId, IFormFile file, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.Get(userId, cancellationToken);
        if (user == null) return false;
        var image = await UploadImage(file);
        await userRepository.UpdateImage(user.Id, image, cancellationToken);
        return true;

    }
    
    private async Task<string> UploadImage(IFormFile file)
    {
        var storageConnectionString = config["AzureBlob_ConnectionString"];
        var containerName = "userimages";

        var blobServiceClient = new BlobServiceClient(storageConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream);
        }

        return $"https://readrstorage.blob.core.windows.net/userimages/{fileName}";
            
    }
}