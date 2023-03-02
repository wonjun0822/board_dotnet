using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using board_dotnet.Service;
using board_dotnet.DTO;
using board_dotnet.Enum;

using System.Net.Mime;

namespace board_dotnet.Controllers
{
    /// <summary>
    /// 게시글 API
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        /// <summary>
        /// 게시글 API
        /// </summary>
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 게시글 목록 조회 (Offset Pagination)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/articles_offset
        ///     {
        ///        "searchType": "title",
        ///        "searchKeyword": "test",        
        ///        "pageIndex": 1,
        ///        "pageSize": 20,
        ///     }
        ///
        /// </remarks>
        /// <param name="searchType">검색 조건 제목(title), 내용(content)</param>
        /// <param name="searchKeyword">검색 Keyword</param>
        /// <param name="pageIndex">조회 할 PageIndex</param>
        /// <param name="pageSize">조회 할 PageSize</param>
        /// <response code="200">게시글 목록 Response / totalPage를 통해 마지막 Page가 어딘지 알 수 있음</response>
        /// <response code="404">게시글 목록을 찾을 수 없음</response>
        /// <response code="500">게시글 목록 조회 중 오류 발생</response>
        [HttpGet("articles_offset")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<OffsetDTO<List<ArticleDTO>?>?>> GetArticlesOffSet(SearchType? searchType, string? searchKeyword, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var articles = await _articleService.GetArticlesOffset(searchType, searchKeyword, pageIndex, pageSize);

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
        /// <param name="cursor">조회 할 기준 cursor(게시글 ID) 정보</param>
        /// <response code="200">게시글 목록 Response / return 받은 cursor 정보로 다음 게시글 목록을 찾을 수 있음 / lastPage = true 면 마지막 게시글 목록</response>
        /// <response code="404">게시글 목록을 찾을 수 없음</response>
        /// <response code="500">게시글 목록 조회 중 오류 발생</response>
        [HttpGet("articles_cursor")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CursorDTO<List<ArticleDTO>?>?>> GetArticlesCursor(long cursor = 0)
        {
            try
            {
                var articles = await _articleService.GetArticlesCursor(cursor);

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
        /// <param name="id">게시글 ID</param>
        /// <response code="200">게시글 Response</response>
        /// <response code="404">게시글을 찾을 수 없음</response>
        /// <response code="500">게시글 조회 중 오류 발생</response>
        [Authorize]
        [HttpGet("articles/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArticleDetailDTO>?> GetArticle(long id)
        {
            try
            {
                var article = await _articleService.GetArticle(id);

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
        ///         "file": "select file"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">11</param>
        /// <response code="201">Header Location 추가된 게시글 URI / Resposne Body 추가된 게시글</response>
        /// <response code="400">게시글 Paramter 잘못된 요청</response>
        /// <response code="500">게시글 추가 중 오류 발생</response>
        [Authorize]
        [HttpPost("articles")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), 400, MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult?> AddArticle([FromForm] ArticleWriteDTO request)
        {
            try
            {
                if (request.title.Trim().Length == 0) {
                    return BadRequest("제목은 빈 값일 수 없습니다.");
                }

                else if (request.content.Trim().Length == 0) {
                    return BadRequest("내용은 빈 값일 수 없습니다.");
                }

                else {
                    var result = await _articleService.AddArticle(request);

                    if (result == null)
                        return Problem("게시글 추가 중 오류가 발생했습니다.");

                    return CreatedAtAction(nameof(GetArticle), new { id = result.id }, result);
                }
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
        ///         "file": "select file"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">게시글 ID</param>
        /// <response code="200">업데이트 성공 / 수정 된 게시글 Response</response>
        /// <response code="404">업데이트 할 게시글을 찾을 수 없음</response>
        /// <response code="500">업데이트 중 오류 발생</response>
        [Authorize]
        [HttpPut("articles/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult?> UpdateArticle(long id, [FromForm] ArticleWriteDTO request)
        {
            try
            {
                if (request.title.Trim().Length == 0) {
                    return BadRequest("제목은 빈 값일 수 없습니다.");
                }

                else if (request.content.Trim().Length == 0) {
                    return BadRequest("내용은 빈 값일 수 없습니다.");
                }

                else {
                    var result = await _articleService.UpdateArticle(id, request);

                    if (result == null)
                        return NotFound("업데이트 할 게시글을 찾을 수 없습니다.");

                    return Ok(result);
                }
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
        /// <param name="id">게시글 ID</param>
        /// <response code="204">삭제 성공</response>
        /// <response code="404">삭제할 게시글을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpDelete("articles/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteArticle(long id)
        {
            try
            {
                var result = await _articleService.DeleteArticle(id);

                if (!result)
                    return NotFound("삭제 할 게시글을 찾을 수 없습니다.");

                return NoContent();
            }

            catch
            {
                return Problem("게시글 삭제 중 오류가 발생했습니다.");
            }
        }
    }
}