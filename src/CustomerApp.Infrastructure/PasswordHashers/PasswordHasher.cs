using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace CustomerApp.Infrastructure.PasswordHashers;

public sealed class PasswordHasher : IPasswordHasher
{
    readonly PasswordHashSettings _passwordHashettings;
    readonly ILogger _logger;

    public PasswordHasher(IOptions<PasswordHashSettings> options, ILogger<PasswordHasher> logger)
    {
        _passwordHashettings = options.Value;
        _logger = logger;
    }

    public string Hash(Password password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
          password.Value,
          _passwordHashettings.SaltSize,
          _passwordHashettings.Iterations,
          HashAlgorithmName.SHA512);
        var key = Convert.ToBase64String(algorithm.GetBytes(_passwordHashettings.KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{_passwordHashettings.Iterations}.{salt}.{key}";
    }

    public bool IsCorrectPassword(CustomerId customerId, HashedPassword savedPassword, Password password)
    {
        var parts = savedPassword.Value.Split('.', 3);
        if (parts.Length != 3)
        {
            _logger.LogError($"The saved hashed password of customer {customerId.Value} has not the right format.");
            return false;
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        using var algorithm = new Rfc2898DeriveBytes(
          password.Value,
          salt,
          iterations,
          HashAlgorithmName.SHA512);
        var keyToCheck = algorithm.GetBytes(_passwordHashettings.KeySize);

        return keyToCheck.SequenceEqual(key);
    }
}
