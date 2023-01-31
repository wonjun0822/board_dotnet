using board_dotnet.Model;
using board_dotnet.DTO;

namespace board_dotnet.Repository
{
    public interface IArticleRepository
    {
        Task<List<ArticleDTO>?> GetArticles();
        Task<ArticleDetailDTO?> GetArticle(long id);
        Task<int> AddArticle(Article article);
        Task<Article?> UpdateArticle(long id, Article request);
        Task<List<Article>?> DeleteArticle(long id);
    }
}