namespace CustomerApp.Application.Handlers.Authentication.Models;

public record RegisterCommand(
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
