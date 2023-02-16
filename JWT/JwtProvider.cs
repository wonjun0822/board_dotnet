using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

using board_dotnet.DTO;

namespace board_dotnet.JWT;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(MemberDTO member)
    {
        var secretKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtOptions.SecretKey));

        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials,
            claims: new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, member.id),
                new Claim(ClaimTypes.Email, member.email),
                new Claim(ClaimTypes.Name, member.nickname)
            }
        );

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
}