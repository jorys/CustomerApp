using CustomerApp.Application.Handlers.Authentication.Interfaces;
using System.Security.Cryptography;

namespace CustomerApp.Infrastructure.TokenGenerators;

public class TokenGenerator : ITokenGenerator
{
    public string GenerateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
