using board_dotnet.DTO;
using board_dotnet.Service;

using Microsoft.AspNetCore.Mvc;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
    }
}