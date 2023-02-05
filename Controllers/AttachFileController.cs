using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using board_dotnet.Service;
using board_dotnet.DTO;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace board_dotnet.Controllers
{
    /// <summary>
    /// 파일
    /// </summary>
    [ApiController]
    [Route("api")]
    public class AttachFileController : ControllerBase
    {
        private readonly IAttachFileService _attachFileService;

        public AttachFileController(IAttachFileService attachFileService)
        {
            _attachFileService = attachFileService;
        }

        /// <summary>
        /// 파일 다운로드
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/files/12
        ///
        /// </remarks>
        /// <response code="200">파일 다운로드</response>
        /// <response code="404">파일을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpGet("files/{fileId}")]
        public async Task<ActionResult> DownloadFile(long fileId)
        {
            try
            {
                var result = await _attachFileService.DownloadFile(fileId);

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
        /// 파일 삭제
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/files/12
        ///
        /// </remarks>
        /// <response code="200">파일 삭제</response>
        /// <response code="404">파일을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [Authorize]
        [HttpDelete("files/{fileId}")]
        public async Task<ActionResult> DeleteFile(long fileId)
        {
            try
            {
                var result = await _attachFileService.DeleteFile(fileId);

                if (result == null)
                    return NotFound("파일을 찾을 수 없습니다.");

                return Ok(result);
            }

            catch
            {
                return Problem("파일 삭제 중 오류가 발생했습니다.");
            }
        }
    }
}