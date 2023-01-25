using board_dotnet.Model;

namespace board_dotnet.Service.ArticleService
{
    public interface IArticleService
    {
        Task<Article?> GetArticle(long id);
        Task<List<Article>> AddArticle(Article article);
        Task<Article?> UpdateArticle(long id, Article request);
        Task<List<Article>?> DeleteArticle(long id);
    }
}