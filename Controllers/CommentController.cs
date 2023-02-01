using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Repository;
using board_dotnet.DTO;
using Microsoft.AspNetCore.Authorization;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [Authorize]
        [HttpGet("comments/{articleId}")]
        public async Task<ActionResult<CommentDTO>?> GetComments(long articleId)
        {
            try
            {
                var comments = await _commentRepository.GetComments(articleId);

                return Ok(comments);
            }

            catch
            {
                return Problem("댓글 조회 중 오류가 발생했습니다.");
            }
        }

        [Authorize]
        [HttpPost("comments/{articleId}")]
        public async Task<ActionResult<int>?> AddComment(long articleId, CommentWriteDTO request)
        {
            try
            {
                var result = await _commentRepository.AddComment(articleId, request);

                if (result == null)
                    return NotFound("댓글을 찾을 수 없습니다.");

                return StatusCode(201, result);
            }

            catch
            {
                return Problem("댓글 추가 중 오류가 발생했습니다.");
            }
        }

        [Authorize]
        [HttpPut("comments/{commentId}")]
        public async Task<ActionResult<Comment>?> UpdateComment(long commentId, CommentWriteDTO comment)
        {
            try
            {
                var result = await _commentRepository.UpdateComment(commentId, comment);

                 if (result == null)
                    return NotFound("댓글을 찾을 수 없습니다.");

                return StatusCode(201, result);
            }

            catch
            {
                return Problem("댓글 업데이트 중 오류가 발생했습니다.");
            }
        }

        [Authorize]
        [HttpDelete("comments/{commentId}")]
        public async Task<ActionResult<Comment>?> DeleteComment(long commentId)
        {
            try
            {
                var result = await _commentRepository.DeleteComment(commentId);

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