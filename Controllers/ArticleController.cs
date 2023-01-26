using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Interface;

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
        public async Task<ActionResult<Article>?> GetArticles()
        {
            var article = await _articleRepository.GetArticles();

            return Ok(article);
        }

        [HttpGet("articles/{id}")]
        public async Task<ActionResult<Article>?> GetArticle(long id)
        {
            var article = await _articleRepository.GetArticle(id);

            return Ok(article);
        }

        [HttpPost("articles")]
        public async Task<ActionResult<int>> AddArticle(Article article)
        {
            var result = await _articleRepository.AddArticle(article);

            return Ok(result);
        }

        [HttpPut("articles/{id}")]
        public async Task<ActionResult<Article>?> UpdateArticle(long id, Article request)
        {
            var result = await _articleRepository.UpdateArticle(id, request);

            return Ok(result);
        }

        [HttpDelete("articles/{id}")]
        public async Task<ActionResult<Article>?> DeleteArticle(long id)
        {
            var result = await _articleRepository.DeleteArticle(id);

            return Ok(result);
        }
    }
}