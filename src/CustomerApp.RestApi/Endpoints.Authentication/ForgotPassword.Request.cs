using CustomerApp.Application.Handlers.Authentication.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.RestApi.Endpoints.Authentication;

public sealed record ForgotPasswordRequest
{
    /// <example>jorys.gaillard@gmail.com</example>
    [EmailAddress]
    public string Email { get; }

    public ForgotPasswordRequest(string email)
    {
        Email = email;
    }

    internal ForgotPasswordCommand ToCommand() =>
        new(Email: Email);
}
