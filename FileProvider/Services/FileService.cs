using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FileProvider.Services;

public class FileService
{
    private readonly ILogger<FileService> _logger;
    private readonly DataContext _dataContext;
    private readonly BlobServiceClient _client;
    private BlobContainerClient _container;

    public FileService(DataContext dataContext, ILogger<FileService> logger, BlobServiceClient client)
    {
        _dataContext = dataContext;
        _logger = logger;
        _client = client;
    }

    public async Task SetBlobContainerAsync(string containerName)
    {
        _container = _client.GetBlobContainerClient(containerName);
        await _container.CreateIfNotExistsAsync();
    }

    public string SetFileName(IFormFile file)
    {
        return $"{Guid.NewGuid().ToString()}_{file.FileName}";
    }

    public async Task<string> UploadFileAsync(  IFormFile file, FileEntity fileEntity)
    {
        BlobHttpHeaders headers = new()
        {
            ContentType = fileEntity.ContentType
        };

        var blobClient = _container.GetBlobClient(fileEntity.FileName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, headers);

        return blobClient.Uri.ToString();
    }

    public async Task SaveToDatabaseAsync(FileEntity fileEntity)
    {
        _dataContext.Files.Add(fileEntity);
        await _dataContext.SaveChangesAsync();
    }
}