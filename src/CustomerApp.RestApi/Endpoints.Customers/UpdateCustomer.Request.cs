using CustomerApp.Application.Handlers.Customers.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.RestApi.Endpoints.Customers;

public sealed record UpdateCustomerRequest
{
    /// <example>Jorys</example>
    public string? FirstName { get; }

    /// <example>Gaillard</example>
    public string? LastName { get; }

    /// <example>2000-04-20</example>
    public DateOnly? Birthdate { get; }

    public AddressRequest? Address { get; }

    public UpdateCustomerRequest(string? firstName = null, string? lastName = null, DateOnly? birthdate = null, AddressRequest? address = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        Address = address;
    }

    public sealed record AddressRequest
    {
        /// <example>1 rue de la Garenne</example>
        public string? Street { get; }

        /// <example>Bruges</example>
        public string? City { get; }

        /// <example>33520</example>
        public int? PostCode { get; }

        /// <example>France</example>
        public string? Country { get; }

        public AddressRequest(string? street = null, string? city = null, int? postCode = null, string? country = null)
        {
            Street = street;
            City = city;
            PostCode = postCode;
            Country = country;
        }
    }

    internal UpdateCustomerCommand ToCommand(Guid customerId) =>
        new(
            CustomerId: customerId,
            FirstName: FirstName,
            LastName: LastName,
            Birthdate: Birthdate,
            Address: new(
                Street: Address?.Street,
                City: Address?.City,
                PostCode: Address?.PostCode,
                Country: Address?.Country));
}
