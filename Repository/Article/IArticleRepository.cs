using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public interface IArticleRepository
    {
        Task<OffsetDTO<List<ArticleDTO>?>?> GetArticlesOffset(int pageIndex, int pageSize);
        Task<CursorDTO<List<ArticleDTO>?>?> GetArticlesCursor(long cursor);
        Task<ArticleDetailDTO?> GetArticle(long id);
        Task<ArticleDetailDTO?> AddArticle(ArticleWriteDTO article);
        Task<ArticleDetailDTO?> UpdateArticle(long id, ArticleWriteDTO request);
        Task<EntityState?> DeleteArticle(long id);
    }
}