using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

using board_dotnet.Interface;
using board_dotnet.Model;

namespace board_dotnet.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(Member member)
    {
        var secretKey = new SymmetricSecurityKey(Convert.FromBase64String(_options.SecretKey));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: DateTime.Now.AddDays(7),
            //expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials,
            claims: new Claim[] {
                //new Claim(JwtRegisteredClaimNames.Sub, member.member_id),
                new Claim(ClaimTypes.NameIdentifier, member.member_id),
                new Claim(ClaimTypes.Name, member.nickname)
            }
        );

        //string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}