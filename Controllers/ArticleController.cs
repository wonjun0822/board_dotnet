using Microsoft.AspNetCore.Mvc;

using board_dotnet.Model;
using board_dotnet.Repository;
using board_dotnet.DTO;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("articlesFilter")]
        public async Task<ActionResult<ArticleDTO>?> GetArticlesFilter()
        {
            var articles = await _articleRepository.GetArticlesFilter();

            return Ok(articles);
        }

        [HttpGet("articles")]
        public async Task<ActionResult<Article>?> GetArticles()
        {
            var articles = await _articleRepository.GetArticles();

            return Ok(articles);
        }

        [Authorize]
        [HttpGet("articles/{id}")]
        public async Task<ActionResult<Article>?> GetArticle(long id)
        {
            var article = await _articleRepository.GetArticle(id);

            return Ok(article);
        }

        [Authorize]
        [HttpGet("articlesFilter/{id}")]
        public async Task<ActionResult<ArticleDetailDTO>?> GetArticleFilter(long id)
        {
            var article = await _articleRepository.GetArticleFilter(id);

            return Ok(article);
        }

        [Authorize]
        [HttpPost("articles")]
        public async Task<ActionResult<int>> AddArticle(Article article)
        {
            var result = await _articleRepository.AddArticle(article);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("articles/{id}")]
        public async Task<ActionResult<Article>?> UpdateArticle(long id, Article request)
        {
            var result = await _articleRepository.UpdateArticle(id, request);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("articles/{id}")]
        public async Task<ActionResult<Article>?> DeleteArticle(long id)
        {
            var result = await _articleRepository.DeleteArticle(id);

            return Ok(result);
        }
    }
}