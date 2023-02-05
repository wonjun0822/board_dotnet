using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Service;
using board_dotnet.DTO;
using Microsoft.AspNetCore.Authorization;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

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
        /// <response code="200">게시글 댓글 목록 Response</response>
        /// <response code="404">게시글 댓글 목록을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpGet("comments/{articleId}")]
        [Produces("application/json")]
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
        ///     POST /api/comments/6
        ///     {
        ///        "comment": "댓글",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">게시글 댓글 목록 Response</response>
        /// <response code="404">댓글을 추가 할 게시글을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpPost("comments/{articleId}")]
        [Produces("application/json")]
        public async Task<ActionResult<int>?> AddComment(long articleId, CommentWriteDTO request)
        {
            try
            {
                var result = await _commentService.AddComment(articleId, request);

                if (result == null)
                    return NotFound("댓글을 추가 할 게시글을 찾을 수 없습니다.");

                return StatusCode(201, result);
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
        ///     PUT /api/comments/131
        ///     {
        ///        "comment": "댓글",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">게시글 댓글 목록 Response</response>
        /// <response code="404">수정 할 댓글 정보를 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpPut("comments/{commentId}")]
        [Produces("application/json")]
        public async Task<ActionResult<Comment>?> UpdateComment(long commentId, CommentWriteDTO comment)
        {
            try
            {
                var result = await _commentService.UpdateComment(commentId, comment);

                 if (result == null)
                    return NotFound("댓글을 찾을 수 없습니다.");

                return StatusCode(201, result);
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
        ///     DELETE /api/comments/131
        ///
        /// </remarks>
        /// <response code="200">삭제 성공</response>
        /// <response code="404">삭제 할 댓글 정보를 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpDelete("comments/{commentId}")]
        public async Task<ActionResult<Comment>?> DeleteComment(long commentId)
        {
            try
            {
                var result = await _commentService.DeleteComment(commentId);

                if (result == null)
                    return NotFound("댓글을 찾을 수 없습니다.");

                return Ok(result);
            }

            catch
            {
                return Problem("댓글 삭제 중 오류가 발생했습니다.");
            }
        }
    }
}