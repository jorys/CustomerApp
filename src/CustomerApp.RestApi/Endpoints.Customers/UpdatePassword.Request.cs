using CustomerApp.Application.Handlers.Customers.Models;

namespace CustomerApp.RestApi.Endpoints.Customers;

public sealed record UpdatePasswordRequest
{
    /// <example>PassW0RD!!</example>
    public string Password { get; }

    public UpdatePasswordRequest(string password)
    {
        Password = password;
    }

    internal UpdatePasswordCommand ToCommand(Guid customerId) =>
        new(
            CustomerId: customerId,
            Password: Password);
}
