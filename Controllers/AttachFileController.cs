using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using board_dotnet.Service;
using System.Net.Mime;

namespace board_dotnet.Controllers
{
    /// <summary>
    /// 첨부파일 API
    /// </summary>
    [ApiController]
    [Route("api")]
    public class AttachFileController : ControllerBase
    {
        private readonly IAttachFileService _attachFileService;

        /// <summary>
        /// 첨부파일 API
        /// </summary>
        public AttachFileController(IAttachFileService attachFileService)
        {
            _attachFileService = attachFileService;
        }

        /// <summary>
        /// 첨부파일 전체 다운로드
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/articles/536/files
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <response code="200">파일 다운로드</response>
        /// <response code="404">파일을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpGet("articles/{articleId}/files")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DownloadFileAll(long articleId)
        {
            try
            {
                var result = await _attachFileService.DownloadFileAll(articleId);

                if (result == null)
                    return NotFound("파일을 찾을 수 없습니다.");

                return File(result.content, result.contentType, result.fileName);
            }

            catch
            {
                return Problem("파일 다운로드 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 첨부파일 다운로드
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/articles/536/files/5
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <param name="fileId">첨부파일 ID</param>
        /// <response code="200">파일 다운로드</response>
        /// <response code="404">파일을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpGet("articles/{articleId}/files/{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DownloadFile(long articleId, long fileId)
        {
            try
            {
                var result = await _attachFileService.DownloadFile(articleId, fileId);

                if (result == null)
                    return NotFound("파일을 찾을 수 없습니다.");

                return File(result.content, result.contentType, result.fileName);
            }

            catch
            {
                return Problem("파일 다운로드 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 첨부파일 전체 삭제
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/articles/536/files/5
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <response code="204">파일 삭제</response>
        /// <response code="404">파일을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpDelete("articles/{articleId}/files")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteFileAll(long articleId)
        {
            try
            {
                var result = await _attachFileService.DeleteFileAll(articleId);

                if (!result)
                    return NotFound("파일을 찾을 수 없습니다.");

                return NoContent();
            }

            catch
            {
                return Problem("파일 삭제 중 오류가 발생했습니다.");
            }
        }

        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/articles/536/files/5
        ///
        /// </remarks>
        /// <param name="articleId">게시글 ID</param>
        /// <param name="fileId">첨부파일 ID</param>
        /// <response code="204">파일 삭제</response>
        /// <response code="404">파일을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpDelete("articles/{articleId}/files/{fileId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteFile(long articleId, long fileId)
        {
            try
            {
                var result = await _attachFileService.DeleteFile(articleId, fileId);

                if (!result)
                    return NotFound("파일을 찾을 수 없습니다.");

                return NoContent();
            }

            catch
            {
                return Problem("파일 삭제 중 오류가 발생했습니다.");
            }
        }
    }
}