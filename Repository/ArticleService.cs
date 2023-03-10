using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.JWT;
using board_dotnet.Model;
using board_dotnet.Service;
using board_dotnet.Enum;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _context;
        private readonly IAuthProvider _authProvider;

        private readonly ICommentService _commentService;
        private readonly IAttachFileService _attachFileService;

        public ArticleService(AppDbContext context, IAuthProvider authProvider, ICommentService commentService, IAttachFileService attachFileService)
        {
            _context = context;
            _authProvider = authProvider;
            _commentService = commentService;
            _attachFileService = attachFileService;
        }

        public async Task<OffsetDTO<List<ArticleDTO>?>?> GetArticlesOffset(SearchType? searchType, string? searchKeyword, int pageIndex, int pageSize)
        {
            try
            {
                var articles = await _context.Articles
                    .AsNoTracking()
                    .Where(e => searchKeyword == null ? true : EF.Property<string>(e, searchType.ToString()).Contains(searchKeyword))
                    .Select(
                        s => new ArticleDTO() { 
                            id = s.id, 
                            title = s.title, 
                            viewCount = s.viewCount,
                            nickname = s.member.nickname,
                            updateAt = s.updateAt
                        }
                    )
                    .OrderByDescending(o => o.id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (articles == null)
                    return null;
                    
                var totalCount = await _context.Articles.AsNoTracking().Where(e => searchKeyword == null ? true : EF.Property<string>(e, searchType.ToString()).Contains(searchKeyword)).CountAsync();

                return new OffsetDTO<List<ArticleDTO>?>(articles, pageIndex, pageSize, (int)Math.Ceiling(((double)totalCount / pageSize)));
            }

            catch
            {
                throw;
            }
        }

        public async Task<CursorDTO<List<ArticleDTO>?>?> GetArticlesCursor(long cursor)
        {
            try
            {
                var articles = await _context.Articles
                    .AsNoTracking()
                    .Select(
                        s => new ArticleDTO() { 
                            id = s.id, 
                            title = s.title, 
                            viewCount = s.viewCount,
                            nickname = s.member.nickname,
                            updateAt = s.updateAt
                        }
                    )
                    .OrderByDescending(o => o.id)
                    .Where(x => cursor == 0 ? true : x.id < cursor)
                    .Take(20)
                    .ToListAsync();

                if (articles == null)
                    return null;

                cursor = articles.Select(x => (int)x.id).LastOrDefault();

                bool lastPage = !(await _context.Articles.Where(x => x.id < cursor).AnyAsync());

                return new CursorDTO<List<ArticleDTO>?>(articles, cursor, lastPage);
            }

            catch
            {
                throw;
            }
        }

        public async Task<ArticleDetailDTO?> GetArticle(long id, bool updateCount = true)
        {
            try
            {
                var articleDetail = await _context.Articles
                    .AsNoTracking()
                    .Select(
                        s => new ArticleDetailDTO() {
                            id = s.id,
                            title = s.title,
                            content = s.content,
                            viewCount = s.viewCount,
                            nickname = s.member.nickname,
                            updateAt = s.updateAt,
                            isModify = s.createBy == _authProvider.GetById(),
                            comments = s.articleComments.Select(
                                o => new CommentDTO() { 
                                    articleId = o.articleId,
                                    commentId = o.id,
                                    comment = o.comment,
                                    nickname = o.member.nickname,
                                    updateAt = s.updateAt,
                                    isModify = s.createBy == _authProvider.GetById()
                                }
                            ).OrderByDescending(o => o.commentId).ToList(),
                            files = s.articleFiles.Select(
                                o => new AttachFileDTO() {
                                    articleId = o.articleId,
                                    attachFileId = o.id,
                                    fileName = o.fileName
                                }
                            ).OrderByDescending(o => o.attachFileId).ToList()
                        }
                    )
                    .FirstOrDefaultAsync(x => x.id == id);

                if (articleDetail == null)
                    return null;

                else if (updateCount)
                {
                    var article = await _context.Articles.FindAsync(articleDetail.id);

                    article.viewCount = article.viewCount + 1;

                    await _context.SaveChangesAsync();
                }

                return articleDetail;
            }

            catch
            {
                throw;
            }
        }

        public async Task<ArticleResultDTO?> AddArticle(ArticleWriteDTO request)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var article = new Article(request.title.Trim(), request.content.Trim());

                    article.member = await _context.Members.Where(x => x.id == _authProvider.GetById()).FirstOrDefaultAsync();

                    _context.Articles.Add(article);

                    await _context.SaveChangesAsync();

                    if (request.files != null) 
                    {
                        var uploadFiles = await _attachFileService.UploadFile(article.id, request.files);

                        if (uploadFiles == null)
                        {
                            await _context.Database.RollbackTransactionAsync();
                        }

                        else
                        {
                            foreach(var uploadFile in uploadFiles)
                            {
                                var attachFile = new AttachFile(article.id, uploadFile.fileName);

                                _context.AttachFiles.Add(attachFile);
                            }
                        }
                    }

                    if (_context.Database.CurrentTransaction != null)
                    {
                        await _context.SaveChangesAsync();

                        await _context.Database.CommitTransactionAsync();

                        return await ArticleResult(article.id);
                    }

                    else
                        return null;
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<ArticleResultDTO?> UpdateArticle(long id, ArticleWriteDTO request)
        {
            try
            {
                var article = await _context.Articles.Where(x => x.id == id && x.createBy == _authProvider.GetById()).FirstOrDefaultAsync();

                if (article is null)
                    return null;

                else 
                {
                    article.title = request.title.Trim();
                    article.content = request.content.Trim();

                    if (request.files != null) 
                    {
                        var uploadFiles = await _attachFileService.UploadFile(article.id, request.files);

                        if (uploadFiles == null)
                        {
                            return null;
                        }

                        else
                        {
                            foreach(var uploadFile in uploadFiles)
                            {
                                var attachFile = new AttachFile(article.id, uploadFile.fileName);

                                _context.AttachFiles.Add(attachFile);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();

                    return await ArticleResult(article.id);
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteArticle(long id)
        {
            try
            {
                var article = await _context.Articles.Where(x => x.id == id && x.createBy == _authProvider.GetById()).Include(s => s.articleComments).Include(s => s.articleFiles).FirstOrDefaultAsync();

                if (article is null)
                    return false;

                else 
                {
                    if (article.articleComments.Count() > 0) 
                        await _commentService.DeleteCommentAll(id);

                    if (article.articleFiles.Count() > 0) 
                        await _attachFileService.DeleteFileAll(id);

                    _context.Articles.Remove(article);

                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            catch
            {
                throw;
            }
        }

        private async Task<ArticleResultDTO> ArticleResult(long id) {
            return await _context.Articles
                .AsNoTracking()
                .Where(x => x.id == id)
                .Select(
                    s => new ArticleResultDTO() {
                        id = s.id,
                        title = s.title,
                        content = s.content,
                        createAt = s.createAt,
                        updateAt = s.updateAt
                    }
                )
                .FirstOrDefaultAsync();
        }
    }
}