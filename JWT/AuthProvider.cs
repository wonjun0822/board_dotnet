using System.Security.Claims;

namespace board_dotnet.JWT;

internal sealed class AuthProvider : IAuthProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetById()
    {
        return _httpContextAccessor?.HttpContext?.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
    }

    public string GetCookie(string cookieName)
    {
        return _httpContextAccessor?.HttpContext?.Request?.Cookies[cookieName]?.ToString()!;
    }
}