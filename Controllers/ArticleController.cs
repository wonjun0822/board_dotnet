using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Service.ArticleService;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("articles/{id}")]
        public async Task<ActionResult<Article>?> GetArticle(long id)
        {
            var article = await _articleService.GetArticle(id);

            return Ok(article);
        }

        [HttpPost("articles")]
        public async Task<ActionResult<Article>> AddArticle(Article article)
        {
            var result = await _articleService.AddArticle(article);

            return Ok(result);
        }

        [HttpPut("articles/{id}")]
        public async Task<ActionResult<Article>?> UpdateArticle(long id, Article request)
        {
            var result = await _articleService.UpdateArticle(id, request);

            return Ok(result);
        }

        [HttpDelete("articles/{id}")]
        public async Task<ActionResult<Article>?> DeleteArticle(long id)
        {
            var result = await _articleService.DeleteArticle(id);

            return Ok(result);
        }
    }
}