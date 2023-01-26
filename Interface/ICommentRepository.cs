using board_dotnet.Model;

namespace board_dotnet.Interface
{
    public interface ICommentRepository
    {
        Task<List<Comment>?> GetComments(long articleId);
        Task<int> AddComment(long articleId, Comment comment);
        Task<Comment?> UpdateComment(long commentId, Comment request);
        Task<List<Comment>?> DeleteComment(long commentId);
    }
}