using board_dotnet.Data;
using board_dotnet.Model;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>?> GetComments(long articleId)
        {
            var comments = await _context.Comments.Where(e => EF.Property<long>(e, "articleId") == articleId).ToListAsync();

            if (comments is null)
                return null;

            return comments;
        }

        public async Task<int> AddComment(long articleId, Comment comment)
        {
            _context.Entry(comment).Property("articleId").CurrentValue = articleId;

            _context.Comments.Add(comment);

            return await _context.SaveChangesAsync();
        }

        public async Task<Comment?> UpdateComment(long commentId, Comment request)
        {
             var comment = await _context.Comments.FindAsync(commentId);

            if (comment is null)
                return null;

            comment.comment = request.comment;
            
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>?> DeleteComment(long commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment is null)
                return null;
                
            _context.Comments.Remove(comment);

            return await _context.Comments.ToListAsync();
        }
    }
}