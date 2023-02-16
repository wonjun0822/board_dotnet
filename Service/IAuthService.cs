using board_dotnet.DTO;

namespace board_dotnet.Service
{
    public interface IAuthService
    {
        Task<TokenDTO?> Login(string id, string pwd);
        Task<TokenDTO?> RefreshToken();
        Task Logout();
    }
}