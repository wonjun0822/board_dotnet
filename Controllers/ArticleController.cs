using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Repository;
using board_dotnet.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpGet("articles")]
        public async Task<ActionResult<ArticleDTO>?> GetArticles()
        {
            try
            {
                var articles = await _articleRepository.GetArticles();

                return Ok(articles);
            }

            catch
            {
                return Problem("게시글 목록 중 오류가 발생했습니다.");
            }
        }

        [Authorize]
        [HttpGet("articles/{id}")]
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

        [Authorize]
        [HttpPost("articles")]
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

        [Authorize]
        [HttpPut("articles/{id}")]
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