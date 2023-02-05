using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service
{
    public interface ICommentService
    {
        Task<List<CommentDTO>?> GetComments(long articleId);
        Task<List<CommentDTO>?> AddComment(long articleId, CommentWriteDTO request);
        Task<List<CommentDTO>?> UpdateComment(long commentId, CommentWriteDTO request);
        Task<EntityState?> DeleteComment(long commentId);
    }
}