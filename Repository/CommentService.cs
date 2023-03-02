using board_dotnet.Data;
using board_dotnet.Model;
using board_dotnet.DTO;
using board_dotnet.Service;
using board_dotnet.JWT;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IAuthProvider _authProvider;
        
        public CommentService(AppDbContext context, IAuthProvider authProvider)
        {
            _context = context;
            _authProvider = authProvider;
        }

        public async Task<List<CommentDTO>?> GetComments(long articleId)
        {
            try
            {
                if (!await _context.Articles.Where(x => x.id == articleId).AnyAsync())
                    return null;
                    
                var comments = await _context.Comments.Where(e => EF.Property<long>(e, "articleId") == articleId).Select(s => new CommentDTO() {
                    articleId = s.articleId,
                    commentId = s.id,
                    comment = s.comment,
                    nickname = s.member.nickname,
                    updateAt = s.updateAt,
                    isModify = s.createBy == _authProvider.GetById(),
                })
                .OrderByDescending(o => o.commentId)
                .ToListAsync();

                return comments;
            }

            catch
            {
                throw;
            }
        }

        public async Task<CommentResultDTO?> AddComment(long articleId, CommentWriteDTO request)
        {
            try
            {
                if (!await _context.Articles.Where(x => x.id == articleId).AnyAsync())
                    return null;

                var comment = new Comment(articleId, request.comment);

                comment.member = await _context.Members.Where(x => x.id == _authProvider.GetById()).FirstOrDefaultAsync();

                _context.Comments.Add(comment);

                await _context.SaveChangesAsync();

                return await CommentResult(comment.id);
            }

            catch
            {
                throw;
            }
        }

        public async Task<CommentResultDTO?> UpdateComment(long articleId, long commentId, CommentWriteDTO request)
        {
            try
            {
                var comment = await _context.Comments.Where(s => s.articleId == articleId && s.id == commentId && s.createBy == _authProvider.GetById()).FirstOrDefaultAsync();

                if (comment is null)
                    return null;

                else 
                {
                    comment.comment = request.comment;
                    
                    await _context.SaveChangesAsync();

                    return await CommentResult(comment.id);
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteCommentAll(long articleId)
        {
            try
            {
                var comments = await _context.Comments.Where(s => s.articleId == articleId && s.createBy == _authProvider.GetById()).FirstOrDefaultAsync();

                if (comments is null)
                    return false;
                    
                else 
                {
                    _context.Comments.RemoveRange(comments);

                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteComment(long commentId)
        {
            try
            {
                var comment = await _context.Comments.Where(s => s.id == commentId && s.createBy == _authProvider.GetById()).FirstOrDefaultAsync();

                if (comment is null)
                    return false;
                    
                else 
                {
                    _context.Comments.Remove(comment);

                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            catch
            {
                throw;
            }
        }

        private async Task<CommentResultDTO> CommentResult(long commentId) {
            return await _context.Comments
                .AsNoTracking()
                .Where(x => x.id == commentId)
                .Select(
                    s => new CommentResultDTO() {
                        articleId = s.articleId,
                        commentId = s.id,
                        comment = s.comment,
                        nickname = s.member.nickname,
                        createAt = s.createAt,
                        updateAt = s.updateAt  
                    }
                )
                .FirstOrDefaultAsync();
        }
    }
}