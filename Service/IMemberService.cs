using board_dotnet.DTO;

namespace board_dotnet.Service
{
    public interface IMemberService
    {
        Task<MemberDTO?> GetMemberById(long id);
        Task<MemberDTO?> GetMemberByPassword(string email, string pwd);
    }
}