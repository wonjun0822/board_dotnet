namespace board_dotnet.JWT;

/// Auth Provider
public interface IAuthProvider
{
    /// 로그인한 사용자 ID 가져오기
    long GetById();

    /// Cookie 정보 가져오기
    string GetCookie(string cookieName);
}
