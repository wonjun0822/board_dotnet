using board_dotnet.Model;

namespace board_dotnet.JWT
{
    public interface IJwtProvider
    {
        string Generate(Member member);
    }
}