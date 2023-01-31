using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

using board_dotnet.Model;

namespace board_dotnet.JWT;

internal sealed class UserResolverProvider : IUserResolverProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserResolverProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetById()
    {
        return _httpContextAccessor?.HttpContext?.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
    }
}