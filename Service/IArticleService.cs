using board_dotnet.DTO;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service
{
    public interface IArticleService
    {
        Task<OffsetDTO<List<ArticleDTO>?>?> GetArticlesOffset(int pageIndex, int pageSize);
        Task<CursorDTO<List<ArticleDTO>?>?> GetArticlesCursor(long cursor);
        Task<ArticleDetailDTO?> GetArticle(long id, bool updateCount = true);
        Task<ArticleDetailDTO?> AddArticle(ArticleWriteDTO article);
        Task<ArticleDetailDTO?> UpdateArticle(long id, ArticleWriteDTO request);
        Task<EntityState?> DeleteArticle(long id);
    }
}