using CustomerApp.Application.Handlers.Authentication.Models;

namespace CustomerApp.RestApi.Endpoints.Authentication.Responses;

public sealed record AuthenticationResponse
{
    /// <example>Jorys</example>
    public string FirstName { get; }

    /// <example>Gaillard</example>
    public string LastName { get; }

    /// <example>jorys.gaillard@gmail.com</example>
    public string Email { get; }

    public string JwtToken { get; }

    public AuthenticationResponse(string firstName, string lastName, string email, string jwtToken)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        JwtToken = jwtToken;
    }

    internal static AuthenticationResponse From(AuthenticationResult authenticationResult) =>
        new AuthenticationResponse(
            firstName: authenticationResult.FirstName.Value,
            lastName: authenticationResult.LastName.Value,
            email: authenticationResult.Email.Value,
            jwtToken: authenticationResult.JwtToken);
}
