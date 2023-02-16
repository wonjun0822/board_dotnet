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
        private readonly IAttachFileService _attachFileService;

        public ArticleService(AppDbContext context, IAuthProvider _authProvider, IAttachFileService attachFileService)
        {
            _context = context;
            _authProvider = _authProvider;
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
                            comments = s.articleComments.Select(
                                o => new CommentDTO() { 
                                    articleId = o.articleId,
                                    commentId = o.id,
                                    comment = o.comment,
                                    nickname = o.member.nickname,
                                    createAt = s.createAt
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

        public async Task<ArticleDetailDTO?> AddArticle(ArticleWriteDTO request)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var article = new Article(request.title, request.content);

                    article.member = await _context.Members.Where(x => x.member_id == _authProvider.GetById()).FirstOrDefaultAsync();

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
                                var attachFile = new AttachFile(article.id, uploadFile.fileName, uploadFile.blobName);

                                _context.AttachFiles.Add(attachFile);
                            }
                        }
                    }

                    if (_context.Database.CurrentTransaction != null)
                    {
                        await _context.SaveChangesAsync();

                        await _context.Database.CommitTransactionAsync();

                        return await GetArticle(article.id, false);
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

        public async Task<ArticleDetailDTO?> UpdateArticle(long id, ArticleWriteDTO request)
        {
            try
            {
                var article = await _context.Articles.Where(x => x.id == id && x.createBy == _authProvider.GetById()).FirstOrDefaultAsync();

                if (article is null)
                    return null;

                else 
                {
                        article.title = request.title;
                        article.content = request.content;

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
                                    var attachFile = new AttachFile(article.id, uploadFile.fileName, uploadFile.blobName);

                                    _context.AttachFiles.Add(attachFile);
                                }
                            }
                        }

                        await _context.SaveChangesAsync();

                        return await GetArticle(article.id, false);
                }
            }

            catch
            {
                throw;
            }
        }

        public async Task<EntityState?> DeleteArticle(long id)
        {
            try
            {
                var article = await _context.Articles.Where(x => x.id == id && x.createBy == _authProvider.GetById()).FirstOrDefaultAsync();

                if (article is null)
                    return null;

                else 
                {
                    _context.Articles.Remove(article);

                    _context.ChangeTracker.DetectChanges();

                    await _context.SaveChangesAsync();

                    return _context.Entry(article).State;
                }
            }

            catch
            {
                throw;
            }
        }
    }
}