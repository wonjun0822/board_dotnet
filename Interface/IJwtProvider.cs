using board_dotnet.Model;

namespace board_dotnet.Interface
{
    public interface IJwtProvider
    {
        string Generate(Member member);
    }
}