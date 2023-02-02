using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using board_dotnet.Repository;
using board_dotnet.DTO;

namespace board_dotnet.Controllers
{
    /// <summary>
    /// 게시글
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        /// <summary>
        /// 게시글 목록 조회 (Offset Pagination)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/articles_offset
        ///     {
        ///        "pageIndex": 1,
        ///        "pageSize": 20,
        ///     }
        ///
        /// </remarks>
        /// <response code="200">게시글 목록 Response / totalPage를 통해 마지막 Page가 어딘지 알 수 있음</response>
        /// <response code="404">게시글 목록을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [HttpGet("articles_offset")]
        [Produces("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<OffsetDTO<List<ArticleDTO>?>?>> GetArticlesOffSet(int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var articles = await _articleRepository.GetArticlesOffset(pageIndex, pageSize);

                if (articles == null)
                    return NotFound("게시글을 찾을 수 없습니다.");

                return Ok(articles);
            }

            catch
            {
                return Problem("게시글 목록 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 게시글 목록 조회 (Cursor Pagination)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/articles_cursor
        ///     {
        ///        "cursor": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200">게시글 목록 Response / return 받은 cursor 정보로 다음 게시글 목록을 찾을 수 있음 / lastPage = true 면 마지막 게시글 목록</response>
        /// <response code="404">게시글 목록을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [HttpGet("articles_cursor")]
        [Produces("application/json")]
        public async Task<ActionResult<CursorDTO<List<ArticleDTO>?>?>> GetArticlesCursor(long cursor = 0)
        {
            try
            {
                var articles = await _articleRepository.GetArticlesCursor(cursor);

                if (articles == null)
                    return NotFound("게시글을 찾을 수 없습니다.");

                return Ok(articles);
            }

            catch
            {
                return Problem("게시글 목록 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 게시글 조회
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/articles/6
        ///
        /// </remarks>
        /// <response code="200">게시글 Response</response>
        /// <response code="404">게시글을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpGet("articles/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ArticleDetailDTO>?> GetArticle(long id)
        {
            try
            {
                var article = await _articleRepository.GetArticle(id);

                if (article == null)
                    return NotFound("게시글을 찾을 수 없습니다.");

                return Ok(article);
            }

            catch
            {
                return Problem("게시글 조회 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 게시글 추가
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/articles
        ///     {
        ///         "title": "제목",
        ///         "content": "내용"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">추가 된 게시글 Response</response>
        /// <response code="404">추가 한 게시글을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpPost("articles")]
        [Produces("application/json")]
        public async Task<ActionResult<ArticleDetailDTO>?> AddArticle([FromBody] ArticleWriteDTO article)
        {
            try
            {
                var result = await _articleRepository.AddArticle(article);

                if (result == null)
                    return NotFound("게시글을 찾을 수 없습니다.");

                return StatusCode(201, result);
            }

            catch
            {
                return Problem("게시글 추가 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 게시글 수정
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/articles/6
        ///     {
        ///         "title": "제목",
        ///         "content": "내용"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">수정 된 게시글 Response</response>
        /// <response code="404">수정할 게시글을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpPut("articles/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ArticleDetailDTO>?> UpdateArticle(long id, ArticleWriteDTO request)
        {
            try
            {
                var result = await _articleRepository.UpdateArticle(id, request);

                if (result == null)
                    return NotFound("게시글을 찾을 수 없습니다.");

                return StatusCode(201, result);
            }

            catch
            {
                return Problem("게시글 업데이트 중 오류가 발생했습니다.");    
            }
        }

        /// <summary>
        /// 게시글 삭제
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/articles/6
        ///     {
        ///         "title": "제목",
        ///         "content": "내용"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">삭제 성공</response>
        /// <response code="404">삭제할 게시글을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpDelete("articles/{id}")]
        public async Task<ActionResult> DeleteArticle(long id)
        {
            try
            {
                var result = await _articleRepository.DeleteArticle(id);

                if (result == null)
                    return NotFound("게시글을 찾을 수 없습니다.");

                return Ok();
            }

            catch
            {
                return Problem("게시글 삭제 중 오류가 발생했습니다.");
            }
        }
    }
}