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

            if (articles is null)
                return null;

            return articles;
        }

        public async Task<ArticleDetailDTO?> GetArticle(long id)
        {
            //var article = await _context.Articles.Include(b => b.articleComments).Include(b => b.member).FirstOrDefaultAsync(m => m.id == id);
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

            if (article is null)
                return null;

            return article;
        }

        public async Task<int> AddArticle(ArticleWriteDTO request)
        {
            var article = new Article(request.title, request.content);

            article.member = await _context.Members.Where(x => x.member_id == _userResolverProvider.GetById()).FirstOrDefaultAsync();

            _context.Articles.Add(article);

            return await _context.SaveChangesAsync();
        }

        public async Task<Article?> UpdateArticle(long id, Article request)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article is null)
                return null;

            article.title = request.title;
            article.content = request.content;

            await _context.SaveChangesAsync();

            return article;
        }

        public async Task<Article?> DeleteArticle(long id)
        {
            var article = await _context.Articles.Where(x => x.id == id).FirstOrDefaultAsync();

            if (article is null)
                return null;

            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();

            return article;
        }
    }
}