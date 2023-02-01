using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public interface IArticleRepository
    {
        Task<List<ArticleDTO>?> GetArticles();
        Task<ArticleDetailDTO?> GetArticle(long id);
        Task<ArticleDetailDTO> AddArticle(ArticleWriteDTO article);
        Task<ArticleDetailDTO?> UpdateArticle(long id, ArticleWriteDTO request);
        Task<EntityState?> DeleteArticle(long id);
    }
}