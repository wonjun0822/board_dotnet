using board_dotnet.DTO;

namespace board_dotnet.Service;

public interface IStorageService
{
    Task<bool> UploadFile(string path, IFormFile file);

    Task<AttachFileDownloadDTO> DownloadDicrectory(string path);

    Task<AttachFileDownloadDTO> DownloadFile(string filePath);

    Task DeleteDirectory(string path);
    
    Task DeleteFile(string filePath);
}
