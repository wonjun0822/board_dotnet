using board_dotnet.DTO;
using board_dotnet.DTO;

namespace board_dotnet.JWT
{
    public interface IJwtProvider
    {
        string GenerateToken(MemberDTO member);
    }
}