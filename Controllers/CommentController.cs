using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Interface;

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

        [HttpGet("comments/{articleId}")]
        public async Task<ActionResult<Comment>?> GetComments(long articleId)
        {
            var comments = await _commentRepository.GetComments(articleId);

            return Ok(comments);
        }

        [HttpPost("comments/{articleId}")]
        public async Task<ActionResult<int>?> AddComment(long articleId, Comment comment)
        {
            var result = await _commentRepository.AddComment(articleId, comment);

            return Ok(result);
        }

        [HttpPut("comments/{id}")]
        public async Task<ActionResult<Comment>?> UpdateComment(long id, Comment comment)
        {
            var result = await _commentRepository.UpdateComment(id, comment);

            return Ok(result);
        }

        [HttpDelete("comments/{id}")]
        public async Task<ActionResult<Comment>?> DeleteComment(long id)
        {
            var result = await _commentRepository.DeleteComment(id);

            return Ok(result);
        }
    }
}