using board_dotnet.DTO;
using board_dotnet.Repository;
using board_dotnet.JWT;

using Microsoft.AspNetCore.Mvc;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IJwtProvider _jwtProvider;

        public MemberController(IMemberRepository memberRepository, IJwtProvider jwtProvider)
        {
            _memberRepository = memberRepository;
            _jwtProvider = jwtProvider;
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/login
        ///     {
        ///         "id": "test1",
        ///         "pwd": "test"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">AccessToken 발급</response>
        /// <response code="404">사용자 정보를 찾을 수 없음</response>
        /// <response code="500">서버 오류</response>
        [HttpPost("login")]
        public async Task<ActionResult?> Login([FromBody] LoginDTO request)
        {
            var member = await _memberRepository.GetMember(request.id, request.pwd);

            if (member == null)
                return NotFound();
                
            string token = _jwtProvider.Generate(member);

            return Ok(token);
        }

        
    }
}