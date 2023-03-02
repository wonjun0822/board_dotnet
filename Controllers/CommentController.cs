using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Service;
using board_dotnet.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;

namespace board_dotnet.Controllers
{
    /// <summary>
    /// 댓글 API
    /// </summary>
    [Tags("댓글")]
    [ApiController]
    [Route("api")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        /// <summary>
        /// 댓글 API
        /// </summary>
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// 게시글 댓글 조회
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/comments/6
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <response code="200">게시글 댓글 목록 Response</response>
        /// <response code="404">게시글 댓글 목록을 찾을 수 없음</response>
        /// <response code="500">댓글 조회 중 오류 발생</response>
        [Authorize]
        [HttpGet("articles/{articleId}/comments")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommentDTO>?> GetComments(long articleId)
        {
            try
            {
                var comments = await _commentService.GetComments(articleId);

                if (comments == null)
                    return NotFound("게시글을 찾을 수 없습니다.");

                return Ok(comments);
            }

            catch
            {
                return Problem("댓글 조회 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 게시글 댓글 추가
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/articles/314/comments
        ///     {
        ///        "comment": "댓글",
        ///     }
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <response code="201">Header Location 추가된 댓글의 게시글 댓글 목록 URI / 추가된 게시글 Response</response>
        /// <response code="404">댓글을 추가 할 게시글을 찾을 수 없음</response>
        /// <response code="500">댓글 추가 중 오류 발생</response>
        [Authorize]
        [HttpPost("articles/{articleId}/comments")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult?> AddComment(long articleId, CommentWriteDTO request)
        {
            try
            {
                var result = await _commentService.AddComment(articleId, request);

                if (result == null)
                    return NotFound("댓글을 추가 할 게시글을 찾을 수 없습니다.");

                return CreatedAtAction(nameof(GetComments), new { articleId = result.articleId }, result);
            }

            catch
            {
                return Problem("댓글 추가 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 댓글 수정
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/articles/314/comments/131
        ///     {
        ///        "comment": "댓글",
        ///     }
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <param name="commentId">댓글 ID</param>
        /// <response code="200">수정 된 댓글 Response</response>
        /// <response code="404">수정 할 댓글 정보를 찾을 수 없음</response>
        /// <response code="500">댓글 업데이트 중 오류 발생</response>
        [Authorize]
        [HttpPut("articles/{articleId}/comments/{commentId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult?> UpdateComment(long articleId, long commentId, CommentWriteDTO comment)
        {
            try
            {
                var result = await _commentService.UpdateComment(articleId, commentId, comment);

                 if (result == null)
                    return NotFound("댓글을 찾을 수 없습니다.");

                return Ok(result);
            }

            catch
            {
                return Problem("댓글 업데이트 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/articles/314/comments/131
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <param name="commentId">댓글 ID</param>
        /// <response code="204">삭제 성공</response>
        /// <response code="404">삭제 할 댓글 정보를 찾을 수 없음</response>
        /// <response code="500">댓글 삭제 중 오류 발생</response>
        [Authorize]
        [HttpDelete("articles/{articleId}/comments/{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult?> DeleteComment(long articleId, long commentId)
        {
            try
            {
                var result = await _commentService.DeleteComment(articleId, commentId);

                if (!result)
                    return NotFound("댓글을 찾을 수 없습니다.");

                return NoContent();
            }

            catch
            {
                return Problem("댓글 삭제 중 오류가 발생했습니다.");
            }
        }
    }
}