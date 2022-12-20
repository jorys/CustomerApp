namespace CustomerApp.Infrastructure.JwtTokenGenerators;

public sealed class JwtSettings
{
    public string Issuer { get; init; } = "";
    public string Audience { get; init; } = "";
    public int ExpiryMinutes { get; init; } = 0;
    public string Secret { get; init; } = "";
}
