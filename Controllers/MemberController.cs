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

        [HttpPost("login")]
        public async Task<ActionResult?> Login([FromBody] LoginDTO request)
        {
            var member = await _memberRepository.GetMember(request.id, request.pwd);

            if (member == null)
                return null;
                
            string token = _jwtProvider.Generate(member);

            return Ok(token);
        }

        
    }
}