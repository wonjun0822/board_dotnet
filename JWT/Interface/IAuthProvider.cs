using board_dotnet.Model;

namespace board_dotnet.JWT
{
    public interface IAuthProvider
    {
        long GetById();
        string GetCookie(string cookieName);
    }
}