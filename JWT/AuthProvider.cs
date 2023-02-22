using System.Security.Claims;

namespace board_dotnet.JWT;

internal sealed class AuthProvider : IAuthProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long GetById()
    {
        string id = _httpContextAccessor?.HttpContext?.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;

        return id == string.Empty ? 0 : Convert.ToInt64(id);
    }

    public string GetCookie(string cookieName)
    {
        return _httpContextAccessor?.HttpContext?.Request?.Cookies[cookieName]?.ToString()!;
    }
}