using board_dotnet.Model;

namespace board_dotnet.Repository
{
    public interface IMemberRepository
    {
        Task<Member?> GetMember(string id, string pwd);
    }
}