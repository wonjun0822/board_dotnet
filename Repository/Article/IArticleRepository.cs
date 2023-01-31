using board_dotnet.Model;
using board_dotnet.DTO;

namespace board_dotnet.Repository
{
    public interface IArticleRepository
    {
        Task<List<ArticleDTO>?> GetArticles();
        Task<ArticleDetailDTO?> GetArticle(long id);
        Task<int> AddArticle(ArticleWriteDTO article);
        Task<Article?> UpdateArticle(long id, Article request);
        Task<Article?> DeleteArticle(long id);
    }
}