using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using board_dotnet.Data;
using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service;

public class AzureStorageService : IAzureStorageService
{
    private readonly AppDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    private readonly string _containerName = "article";

    public AzureStorageService(AppDbContext context)
    {
        _context = context;
        _blobServiceClient = new BlobServiceClient(new Uri("https://cs1100320026f3b9096.blob.core.windows.net/article?sp=rwdl&st=2023-02-05T11:49:05Z&se=2024-02-05T19:49:05Z&sv=2021-06-08&sr=c&sig=JDvhfzKHMx3PPC6MKGciQjFWvMeGtMt55632wA3f8KA%3D"), null);
    }

    public async Task<BlobContentInfo?> UploadFile(IFormFile file, string uploadFileName) 
    {
        try
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blob = container.GetBlobClient(uploadFileName);

            await using (Stream? data = file?.OpenReadStream())
            {
                return await blob.UploadAsync(data, false);
            }
        }

        // 해당 Container에 동일한 Blob File 있을 시 에러
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            return null;
        }

        // 그외 에러
        catch (RequestFailedException ex)
        {
            return null;
        }
    }

    public async Task<BlobDownloadStreamingResult?> DownloadFile(string blobName)
    {
        AttachFileDownloadDTO result = new();

        try
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blob = container.GetBlobClient(blobName);

            using (BlobDownloadStreamingResult downloadResult = await blob.DownloadStreamingAsync())
            {
                return downloadResult;
            }
        }

        // 파일 없을시 에러
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            return null;
        }
    }

    public async Task DeleteFile(string blobName)
    {
        try
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blob = container.GetBlobClient(blobName);

            await blob.DeleteAsync();
        }

        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
        }
    }
}