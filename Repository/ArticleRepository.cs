using board_dotnet.Data;
using board_dotnet.Interface;
using board_dotnet.Model;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Articles>?> GetArticlesFilter()
        {
            var articles = await _context.Articles.Select(s => new Articles() { id = s.id, title = s.title, viewCount = s.viewCount }).OrderByDescending(o => o.id).ToListAsync();

            if (articles is null)
                return null;

            return articles;
        }

        public async Task<List<Article>?> GetArticles()
        {
            var articles = await _context.Articles.ToListAsync();

            if (articles is null)
                return null;

            return articles;
        }

        public async Task<Article?> GetArticle(long id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article is null)
                return null;

            article.articleComments = await _context.Comments.Where(e => EF.Property<long>(e, "articleId") == article.id).ToListAsync();

            return article;
        }

        public async Task<int> AddArticle(Article article)
        {
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
            article.hashTag = request.hashTag;
            
            await _context.SaveChangesAsync();

            return article;
        }

        public async Task<List<Article>?> DeleteArticle(long id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article is null)
                return null;
                
            _context.Articles.Remove(article);

            return await _context.Articles.ToListAsync();
        }
    }
}