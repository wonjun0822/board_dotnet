using System.Net.Mime;
using board_dotnet.DTO;
using board_dotnet.Service;

using Microsoft.AspNetCore.Mvc;

namespace board_dotnet.Controllers
{
    /// <summary>
    /// 인증 API
    /// </summary>
    [Tags("인증")]
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRedisService _rediseService;

        /// <summary>
        /// 인증 API
        /// </summary>
        public AuthController(IAuthService authService, IRedisService redisService)
        {
            _authService = authService;
            _rediseService = redisService;
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/auth
        ///     {
        ///         "email": "test1@test.com",
        ///         "pwd": "test"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Access Token Response</response>
        /// <response code="404">사용자 정보를 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult?> Login([FromBody] LoginDTO request)
        {
            var token = await _authService.Login(request.email, request.pwd);

            if (token == null)
                return NotFound();

            SetRefreshTokenCookie(token.refreshToken);

            return Ok(token.accessToken);
        }

        /// <summary>
        /// 토근 재발급
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/auth
        ///
        /// </remarks>
        /// <response code="200">Token 재발급</response>
        /// <response code="404">재발급 Token을 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [HttpPut("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), 404, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult?> RefreshToken()
        {
            var token = await _authService.RefreshToken();

            if (token == null)
                return NotFound();

            SetRefreshTokenCookie(token.refreshToken);

            return Ok(token);
        }

        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/auth
        ///
        /// </remarks>
        /// <response code="200">RefreshToken 삭제</response>
        /// <response code="500">서버 오류</response>
        [HttpDelete("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Logout()
        {
            await _authService.Logout();

            Response.Cookies.Delete("refreshToken");

            return Ok();
        }

        private void SetRefreshTokenCookie(string token) {
            var cookieOptions = new CookieOptions {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}