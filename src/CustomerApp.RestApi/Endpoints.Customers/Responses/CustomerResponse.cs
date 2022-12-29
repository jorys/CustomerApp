using CustomerApp.Domain.Aggregates.Customers;

namespace CustomerApp.RestApi.Endpoints.Customers.Responses;

public sealed record CustomerResponse
{
    /// <example>Jorys</example>
    public string FirstName { get; }

    /// <example>Gaillard</example>
    public string LastName { get; }

    /// <example>2000-04-20</example>
    public DateOnly Birthdate { get; }

    /// <example>jorys.gaillard@gmail.com</example>
    public string Email { get; }

    public AddressResponse Address { get; }

    public CustomerResponse(string firstName, string lastName, DateOnly birthdate, string email, AddressResponse address)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        Email = email;
        Address = address;
    }

    public sealed record AddressResponse
    {
        /// <example>1 rue de la Garenne</example>
        public string Street { get; }

        /// <example>Bruges</example>
        public string City { get; }

        /// <example>33520</example>
        public int PostCode { get; }

        /// <example>France</example>
        public string Country { get; }

        public AddressResponse(string street, string city, int postCode, string country)
        {
            Street = street;
            City = city;
            PostCode = postCode;
            Country = country;
        }
    }

    public static CustomerResponse From(Customer customer) =>
        new(
            firstName: customer.FirstName.Value,
            lastName: customer.LastName.Value,
            birthdate: customer.Birthdate.Value,
            email: customer.Email.Value,
            address: new AddressResponse(
                street: customer.Address.Street,
                city: customer.Address.City,
                postCode: customer.Address.PostCode,
                country: customer.Address.Country
        ));
}
