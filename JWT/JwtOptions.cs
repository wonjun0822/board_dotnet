namespace board_dotnet.JWT;

/// JWT Setup Options
public class JwtOptions
{
    /// JWT Option Issuer
    public string Issuer { get; init; } = string.Empty;

    /// JWT Option Audience
    public string Audience { get; init; } = string.Empty;

    /// JWT Option SecretKey
    public string SecretKey { get; init; } = string.Empty;
}