using board_dotnet.DTO;

namespace board_dotnet.Service
{
    public interface IMemberService
    {
        Task<MemberDTO?> GetMemberById(string id);
        Task<MemberDTO?> GetMemberByPassword(string id, string pwd);
    }
}