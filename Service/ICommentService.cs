using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service
{
    public interface ICommentService
    {
        Task<List<CommentDTO>?> GetComments(long articleId);
        Task<CommentResultDTO?> AddComment(long articleId, CommentWriteDTO request);
        Task<CommentResultDTO?> UpdateComment(long articleId, long commentId, CommentWriteDTO request);
        Task<EntityState?> DeleteComment(long commentId);
    }
}