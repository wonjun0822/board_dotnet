using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace board_dotnet.Authentication;

public class JwtBearerOptionSetup : IConfigureOptions<JwtBearerOptions>
{
    private const string SectionName = "Jwt";
    private readonly JwtOptions _jwtOptions;

    public JwtBearerOptionSetup(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = _jwtOptions.Audience,
            ValidIssuer = _jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtOptions.SecretKey)),
            RequireExpirationTime = true
        };
    }
}