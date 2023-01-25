using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Service.ArticleService;
using board_dotnet.Data;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api")]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("comments/{id}")]
        public async Task<ActionResult<Comment>?> GetComment(long id)
        {
            var comment = await _context.Comments.FindAsync(id);

            return Ok(comment);
        }
    }
}