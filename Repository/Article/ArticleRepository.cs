using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.JWT;
using board_dotnet.Model;

using Microsoft.EntityFrameworkCore;

using MR.AspNetCore.Pagination;

namespace board_dotnet.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserResolverProvider _userResolverProvider;

        private readonly IPaginationService _pagnationService;

        public ArticleRepository(AppDbContext context, IUserResolverProvider userResolverProvider, IPaginationService paginationService)
        {
            _context = context;

            _userResolverProvider = userResolverProvider;
            _pagnationService = paginationService;
        }

        public async Task<OffsetDTO<List<ArticleDTO>?>?> GetArticlesOffset(int pageIndex, int pageSize)
        {
            try
            {
                // var articles = await _context.Articles.Include(b => b.articleComments).ToListAsync();
                var articles = await _context.Articles
                    .AsNoTracking()
                    .Select(
                        s => new ArticleDTO() { 
                            id = s.id, 
                            title = s.title, 
                            viewCount = s.viewCount,
                            nickname = s.member.nickname,
                            updateAt = s.updateAt
                        }
                    )
                    .OrderByDescending(o => o.id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (articles == null)
                    return null;
                    
                var totalCount = await _context.Articles.CountAsync();

                return new OffsetDTO<List<ArticleDTO>?>(articles, pageIndex, pageSize, (int)Math.Ceiling(((double)totalCount / pageSize)));
            }

            catch
            {
                throw;
            }
        }

        public async Task<CursorDTO<List<ArticleDTO>?>?> GetArticlesCursor(long cursor)
        {
            try
            {
                var articles = await _context.Articles
                    .AsNoTracking()
                    .Select(
                        s => new ArticleDTO() { 
                            id = s.id, 
                            title = s.title, 
                            viewCount = s.viewCount,
                            nickname = s.member.nickname,
                            updateAt = s.updateAt
                        }
                    )
                    .OrderByDescending(o => o.id)
                    .Where(x => cursor == 0 ? true : x.id < cursor)
                    .Take(20)
                    .ToListAsync();

                if (articles == null)
                    return null;

                cursor = articles.Select(x => (int)x.id).LastOrDefault();

                bool lastPage = !(await _context.Articles.Where(x => x.id < cursor).AnyAsync());

                return new CursorDTO<List<ArticleDTO>?>(articles, cursor, lastPage);
            }

            catch
            {
                throw;
            }
        }

        public async Task<ArticleDetailDTO?> GetArticle(long id)
        {
            try
            {
                //var article = await _context.Articles.Include(b => b.articleComments).Include(b => b.member).FirstOrDefaultAsync(m => m.id == id);

                var article = await _context.Articles.Where(x => x.id == id).FirstOrDefaultAsync();

                if (article is null)
                    return null;

                article.viewCount = article.viewCount + 1;

                await _context.SaveChangesAsync();

                var articleDetail = new ArticleDetailDTO() {
                    id = article.id,
                    title = article.title,
                    content = article.content,
                    viewCount = article.viewCount,
                    nickname = article.member.nickname,
                    updateAt = article.updateAt,
                    comments = article.articleComments.Select(
                        o => new CommentDTO() { 
                            commentId = o.id,
                            comment = o.comment,
                            nickname = o.member.nickname,
                            createAt = article.createAt
                        }
                    ).OrderByDescending(o => o.commentId).ToList()
                };

                // var article = await _context.Articles.Select(
                //     s => new ArticleDetailDTO() {
                //         id = s.id,
                //         title = s.title,
                //         content = s.content,
                //         viewCount = s.viewCount,
                //         nickname = s.member.nickname,
                //         createAt = s.createAt,
                //         comments = s.articleComments.Select(
                //             o => new CommentDTO() { 
                //                 commentId = o.id,
                //                 comment = o.comment,
                //                 nickname = o.member.nickname,
                //                 createAt = s.createAt
                //             }
                //         ).ToList()
                //     }
                // ).FirstOrDefaultAsync(m => m.id == id);

                return articleDetail;
            }

            catch
            {
                throw;
            }
        }

        public async Task<ArticleDetailDTO?> AddArticle(ArticleWriteDTO request)
        {
            try
            {
                var article = new Article(request.title, request.content);

                article.member = await _context.Members.Where(x => x.member_id == _userResolverProvider.GetById()).FirstOrDefaultAsync();

                _context.Articles.Add(article);

                await _context.SaveChangesAsync();

                return await GetArticle(article.id);
            }

            catch
            {
                throw;
            }
        }

        public async Task<ArticleDetailDTO?> UpdateArticle(long id, ArticleWriteDTO request)
        {
            try
            {
                var article = await _context.Articles.Where(x => x.id == id && x.createBy == _userResolverProvider.GetById()).FirstOrDefaultAsync();

                if (article is null)
                    return null;

                else 
                {
                    article.title = request.title;
                    article.content = request.content;

                    await _context.SaveChangesAsync();

                    return await GetArticle(article.id);
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<EntityState?> DeleteArticle(long id)
        {
            try
            {
                var article = await _context.Articles.Where(x => x.id == id && x.createBy == _userResolverProvider.GetById()).FirstOrDefaultAsync();

                if (article is null)
                    return null;

                else 
                {
                    _context.Articles.Remove(article);

                    _context.ChangeTracker.DetectChanges();

                    await _context.SaveChangesAsync();

                    return _context.Entry(article).State;
                }
            }

            catch
            {
                throw;
            }
        }
    }
}