using board_dotnet.Model;
using board_dotnet.DTO;

namespace board_dotnet.Interface
{
    public interface IArticleRepository
    {
        Task<List<ArticleDTO>?> GetArticlesFilter();
        Task<List<Article>?> GetArticles();
        Task<Article?> GetArticle(long id);
        Task<ArticleDetailDTO?> GetArticleFilter(long id);
        Task<int> AddArticle(Article article);
        Task<Article?> UpdateArticle(long id, Article request);
        Task<List<Article>?> DeleteArticle(long id);
    }
}