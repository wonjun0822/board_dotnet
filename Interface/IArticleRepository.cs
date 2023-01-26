using board_dotnet.Model;

namespace board_dotnet.Interface
{
    public interface IArticleRepository
    {
        Task<List<Article>?> GetArticles();
        Task<Article?> GetArticle(long id);
        Task<int> AddArticle(Article article);
        Task<Article?> UpdateArticle(long id, Article request);
        Task<List<Article>?> DeleteArticle(long id);
    }
}