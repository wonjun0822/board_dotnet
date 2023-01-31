using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

using board_dotnet.Model;

namespace board_dotnet.JWT;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string Generate(Member member)
    {
        var secretKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials,
            claims: new Claim[] {
                //new Claim(JwtRegisteredClaimNames.Sub, member.member_id),
                new Claim(ClaimTypes.NameIdentifier, member.member_id),
                new Claim(ClaimTypes.Name, member.nickname)
            }
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}