using Azure.Storage.Blobs.Models;

namespace board_dotnet.Service;

public interface IAzureStorageService
{
    Task<BlobContentInfo> UploadFile(IFormFile file, string uploadFileName);

    Task<BlobDownloadStreamingResult> DownloadFile(string blobName);

    Task DeleteFile(string blobName);

    // /// <summary>
    // /// This method returns a list of all files located in the container
    // /// </summary>
    // /// <returns>Blobs in a list</returns>
    // Task<List<BlobDto>> ListAsync();
}
