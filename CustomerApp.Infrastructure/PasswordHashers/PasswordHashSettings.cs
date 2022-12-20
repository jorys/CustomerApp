namespace CustomerApp.Infrastructure.PasswordHashers;

public sealed class PasswordHashSettings
{
    public int Iterations { get; init; }
    public int SaltSize { get; init; }
    public int KeySize { get; init; }
}