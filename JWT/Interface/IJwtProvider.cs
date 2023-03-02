using board_dotnet.DTO;

namespace board_dotnet.JWT;

/// JWT Provider
public interface IJwtProvider
{
    /// Token Create
    string GenerateToken(MemberDTO member);
}