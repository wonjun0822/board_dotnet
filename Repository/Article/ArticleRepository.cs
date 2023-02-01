using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.JWT;
using board_dotnet.Model;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserResolverProvider _userResolverProvider;

        public ArticleRepository(AppDbContext context, IUserResolverProvider userResolverProvider)
        {
            _context = context;
            _userResolverProvider = userResolverProvider;
        }

        public async Task<List<ArticleDTO>?> GetArticles()
        {
            try
            {
                // var articles = await _context.Articles.Include(b => b.articleComments).ToListAsync();
                var articles = await _context.Articles
                    .Select(
                        s => new ArticleDTO() { 
                            id = s.id, 
                            title = s.title, 
                            viewCount = s.viewCount,
                            nickname = s.member.nickname,
                            createAt = s.createAt
                        }
                    )
                    .OrderByDescending(o => o.id)
                    .ToListAsync();

                return articles;
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

                if (await _context.Articles.FirstOrDefaultAsync(x => x.id == id) == null)
                    return null;

                var article = await _context.Articles.Select(
                    s => new ArticleDetailDTO() {
                        id = s.id,
                        title = s.title,
                        content = s.content,
                        viewCount = s.viewCount,
                        nickname = s.member.nickname,
                        createAt = s.createAt,
                        comments = s.articleComments.Select(
                            o => new CommentDTO() { 
                                commentId = o.id,
                                comment = o.comment,
                                nickname = o.member.nickname,
                                createAt = s.createAt
                            }
                        ).ToList()
                    }
                ).FirstOrDefaultAsync(m => m.id == id);

                return article;
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