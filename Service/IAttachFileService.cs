using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service;

public interface IAttachFileService
{
    Task<List<AttachFileUploadDTO>> UploadFile(long articleId, ICollection<IFormFile>? files);

    Task<AttachFileDownloadDTO> DownloadFile(long fileId);

    Task<EntityState?> DeleteFile(long fileId);
}
