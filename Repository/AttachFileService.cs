using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.JWT;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service;

public class AttachFileService : IAttachFileService
{
    private readonly AppDbContext _context;
    
    private readonly IStorageService _storageService;
    private readonly IAuthProvider _authProvider;

    public AttachFileService(AppDbContext context, IStorageService storageService, IAuthProvider authProvider)
    {
        _context = context;
        _storageService = storageService;
        _authProvider = authProvider;
    }

    public async Task<List<AttachFileUploadDTO>> UploadFile(long articleId, IFormFileCollection? files)
    {
        try
        {
            List<AttachFileUploadDTO>? attachFiles = new();

            foreach (IFormFile file in files)
            {
                if (await _storageService.UploadFile(string.Format("articles/{0}", articleId), file))
                {
                    attachFiles.Add(new AttachFileUploadDTO() {
                        fileName = file.FileName
                    });
                }

                else
                {                
                    await _storageService.DeleteDirectory(string.Format("articles/{0}", articleId));

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

    public async Task<AttachFileDownloadDTO?> DownloadFile(long articleId, long fileId)
    {
        try
        {
            var attachFile = await _context.AttachFiles.FirstOrDefaultAsync(x => x.articleId == articleId && x.id == fileId);

            if (attachFile == null)
                return null;

            return await _storageService.DownloadFile(string.Format("articles/{0}/{1}", articleId, attachFile.fileName));
        }

        catch
        {
            throw;
        }
    }

    public async Task<AttachFileDownloadDTO?> DownloadFileAll(long articleId)
    {
        try
        {
            var attachFile = await _context.AttachFiles.FirstOrDefaultAsync(x => x.articleId == articleId);

            if (attachFile == null)
                return null;

            return await _storageService.DownloadDicrectory(string.Format("articles/{0}", articleId));
        }

        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteFile(long articleId, long fileId)
    {
        try
        {
            var attachFile = await _context.AttachFiles.Where(s => s.articleId == articleId && s.id == fileId && s.createBy == _authProvider.GetById()).FirstOrDefaultAsync();

            if (attachFile is null)
                return false;
                
            else 
            {
                await _storageService.DeleteFile(string.Format("articles/{0}/{1}", articleId, attachFile.fileName));

                _context.AttachFiles.Remove(attachFile);

                await _context.SaveChangesAsync();

                return true;
            }
        }

        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteFileAll(long articleId)
    {
        try
        {
            var attachFiles = await _context.AttachFiles.Where(s => s.articleId == articleId && s.createBy == _authProvider.GetById()).ToListAsync();

            if (attachFiles is null)
                return false;
                
            else 
            {
                await _storageService.DeleteDirectory(string.Format("articles/{0}", articleId));

                _context.AttachFiles.RemoveRange(attachFiles);

                await _context.SaveChangesAsync();

                return true;
            }
        }

        catch
        {
            throw;
        }
    }
}