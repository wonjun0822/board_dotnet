using board_dotnet.DTO;
using board_dotnet.Enum;
using board_dotnet.Model;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Service
{
    public interface IArticleService
    {
        Task<OffsetDTO<List<ArticleDTO>?>?> GetArticlesOffset(SearchType? searchType, string? searchKeyword, int pageIndex, int pageSize);
        Task<CursorDTO<List<ArticleDTO>?>?> GetArticlesCursor(long cursor);
        Task<ArticleDetailDTO?> GetArticle(long id, bool updateCount = true);
        Task<ArticleResultDTO?> AddArticle(ArticleWriteDTO article);
        Task<ArticleResultDTO?> UpdateArticle(long id, ArticleWriteDTO request);
        Task<EntityState?> DeleteArticle(long id);
    }
}