using CustomerApp.Application.Handlers.Authentication.Models;

namespace CustomerApp.Application.Handlers.Customers.Models;

public record UpdateCustomerCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    DateOnly Birthdate,
    string Email,
    string Password,
    RegisterCommand.AddressCommand Address)
{
    public record AddressCommand(
        string Street,
        string City,
        int PostCode,
        string Country);
}
