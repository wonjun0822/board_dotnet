using board_dotnet.DTO;

namespace board_dotnet.Service;

public interface IAttachFileService
{
    Task<List<AttachFileUploadDTO>> UploadFile(long articleId, IFormFileCollection? files);

    Task<AttachFileDownloadDTO> DownloadFile(long articleId, long fileId);

    Task<AttachFileDownloadDTO> DownloadFileAll(long articleId);

    Task<bool> DeleteFile(long articleId, long fileId);

    Task<bool> DeleteFileAll(long articleId);
}
