using board_dotnet.Model;

namespace board_dotnet.Service
{
    public interface IMemberService
    {
        Task<Member?> GetMember(string id, string pwd);
    }
}