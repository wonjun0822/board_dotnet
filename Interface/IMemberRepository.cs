using board_dotnet.Model;
using board_dotnet.DTO;

namespace board_dotnet.Interface
{
    public interface IMemberRepository
    {
        Task<Member?> GetMember(string id, string pwd);
    }
}