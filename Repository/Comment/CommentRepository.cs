using board_dotnet.Data;
using board_dotnet.Model;
using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;
using board_dotnet.JWT;

namespace board_dotnet.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserResolverProvider _userResolverProvider;
        
        public CommentRepository(AppDbContext context, IUserResolverProvider userResolverProvider)
        {
            _context = context;
            _userResolverProvider = userResolverProvider;
        }

        public async Task<List<CommentDTO>?> GetComments(long articleId)
        {
            try
            {
                var comments = await _context.Comments.Where(e => EF.Property<long>(e, "articleId") == articleId).Select(s => new CommentDTO() {
                    articleId = s.articleId,
                    commentId = s.id,
                    comment = s.comment,
                    nickname = s.member.nickname,
                    createAt = s.createAt
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

        public async Task<List<CommentDTO>?> AddComment(long articleId, CommentWriteDTO request)
        {
            try
            {
                var comment = new Comment(articleId, request.comment);

                comment.member = await _context.Members.Where(x => x.member_id == _userResolverProvider.GetById()).FirstOrDefaultAsync();

                _context.Comments.Add(comment);

                await _context.SaveChangesAsync();

                return await GetComments(articleId);
            }

            catch
            {
                throw;
            }
        }

        public async Task<List<CommentDTO>?> UpdateComment(long commentId, CommentWriteDTO request)
        {
            try
            {
                var comment = await _context.Comments.Where(s => s.id == commentId && s.createBy == _userResolverProvider.GetById()).FirstOrDefaultAsync();

                if (comment is null)
                    return null;

                else 
                {
                    comment.comment = request.comment;
                    
                    await _context.SaveChangesAsync();

                    return await GetComments(comment.articleId);
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<EntityState?> DeleteComment(long commentId)
        {
            try
            {
                var comment = await _context.Comments.Where(s => s.id == commentId && s.createBy == _userResolverProvider.GetById()).FirstOrDefaultAsync();

                if (comment is null)
                    return null;
                    
                else 
                {
                    _context.Comments.Remove(comment);

                    await _context.SaveChangesAsync();

                    return _context.Entry(comment).State;
                }
            }

            catch
            {
                throw;
            }
        }
    }
}