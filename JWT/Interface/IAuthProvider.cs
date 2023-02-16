using board_dotnet.Model;

namespace board_dotnet.JWT
{
    public interface IAuthProvider
    {
        string GetById();
        string GetCookie(string cookieName);
    }
}