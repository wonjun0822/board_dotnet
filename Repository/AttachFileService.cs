using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.JWT;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service;

public class AttachFileService : IAttachFileService
{
    private readonly AppDbContext _context;
    
    private readonly IAzureStorageService _azureStorageService;
    private readonly IUserResolverProvider _userResolverProvider;

    public AttachFileService(AppDbContext context, IAzureStorageService azureStorageService, IUserResolverProvider userResolverProvider)
    {
        _context = context;
        _azureStorageService = azureStorageService;
        _userResolverProvider = userResolverProvider;
    }

    public async Task<List<AttachFileUploadDTO>> UploadFile(long articleId, ICollection<IFormFile>? files)
    {
        try
        {
            List<AttachFileUploadDTO>? attachFiles = new();

            foreach (IFormFile file in files)
            {
                string uploadFileName = Path.GetFileNameWithoutExtension(file?.FileName) + "_" + DateTime.Now.ToString("yyymmddHHmmss") + Path.GetExtension(file?.FileName);

                var blob = await _azureStorageService.UploadFile(file, uploadFileName);

                if (blob != null)
                {
                    attachFiles.Add(new AttachFileUploadDTO() {
                        fileName = file.FileName,
                        blobName = uploadFileName
                    });
                }

                else
                {                
                    foreach (var item in attachFiles)
                    {
                        _azureStorageService.DeleteFile(item.blobName);

                        attachFiles.Remove(item);
                    }

                    break;
                }
            }

            return attachFiles;
        }

        catch
        {
            throw;
        }
    }

    public async Task<AttachFileDownloadDTO?> DownloadFile(long fileId)
    {
        try
        {
            var attachFile = await _context.AttachFiles.FirstOrDefaultAsync(x => x.id == fileId);

            if (attachFile == null)
                return null;

            var result = await _azureStorageService.DownloadFile(attachFile.blobName);

            return new AttachFileDownloadDTO() { content = result.Content, contentType = result.Details.ContentType, fileName = attachFile.fileName };
        }

        catch
        {
            throw;
        }
    }

    public async Task<EntityState?> DeleteFile(long fileId)
    {
        try
        {
            var attachFile = await _context.AttachFiles.Where(s => s.id == fileId && s.createBy == _userResolverProvider.GetById()).FirstOrDefaultAsync();

            if (attachFile is null)
                return null;
                
            else 
            {
                _context.AttachFiles.Remove(attachFile);

                await _context.SaveChangesAsync();

                return _context.Entry(attachFile).State;
            }
        }

        catch
        {
            throw;
        }
    }
}