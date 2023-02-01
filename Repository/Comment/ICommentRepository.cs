using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public interface ICommentRepository
    {
        Task<List<CommentDTO>?> GetComments(long articleId);
        Task<List<CommentDTO>?> AddComment(long articleId, CommentWriteDTO request);
        Task<List<CommentDTO>?> UpdateComment(long commentId, CommentWriteDTO request);
        Task<EntityState?> DeleteComment(long commentId);
    }
}