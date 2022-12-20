using CustomerApp.Application.Handlers.Authentication.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.RestApi.Endpoints.Authentication;

public sealed record LoginRequest
{
    /// <example>jorys.gaillard@gmail.com</example>
    [EmailAddress]
    public string Email { get; }

    /// <example>P@sSw0rD!</example>
    [Required]
    public string Password { get; }

    public LoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }

    internal LoginCommand ToCommand() =>
        new(
            Email: Email,
            Password: Password);
}
