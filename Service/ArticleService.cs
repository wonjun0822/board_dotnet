using board_dotnet.Data;
using board_dotnet.Model;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _context;

        public ArticleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Article?> GetArticle(long id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article is null)
                return null;

            await _context.Comments.Where(e => e.articleId == article.id).ToListAsync();

            return article;
        }

        public async Task<List<Article>> AddArticle(Article article)
        {
            _context.Articles.Add(article);

            await _context.SaveChangesAsync();

            return await _context.Articles.ToListAsync();
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