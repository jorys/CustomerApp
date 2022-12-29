using CustomerApp.Application.Handlers.Authentication.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.Application.Handlers.Authentication;

public sealed record ResetPasswordRequest
{
    /// <example>jorys.gaillard@gmail.com</example>
    [EmailAddress]
    public string Email { get; }

    /// <example>token-received-by-email</example>
    public string Token { get; }

    /// <example>PassW0RD!!</example>
    public string Password { get; }

    public ResetPasswordRequest(string email, string token, string password)
    {
        Email = email;
        Token = token;
        Password = password;
    }

    internal ResetPasswordCommand ToCommand() =>
        new(
            Email: Email,
            Token: Token,
            Password: Password);
}