using CustomerApp.Application.Handlers.Authentication.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.RestApi.Endpoints.Authentication;

public sealed record RegisterRequest
{
    /// <example>Jorys</example>
    [Required]
    public string FirstName { get; }

    /// <example>Gaillard</example>
    [Required]
    public string LastName { get; }

    /// <example>2000-04-20</example>
    public DateOnly Birthdate { get; }

    /// <example>jorys.gaillard@gmail.com</example>
    [EmailAddress]
    public string Email { get; }

    /// <example>P@sSw0rD!</example>
    [Required]
    public string Password { get; }

    [Required]
    public AddressRequest Address { get; }

    public RegisterRequest(string firstName, string lastName, DateOnly birthdate, string email, string password, AddressRequest address)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        Email = email;
        Password = password;
        Address = address;
    }

    public sealed record AddressRequest
    {
        /// <example>1 rue de la Garenne</example>
        [Required]
        public string Street { get; }

        /// <example>Bruges</example>
        [Required]
        public string City { get; }

        /// <example>33520</example>
        [Required]
        public int PostCode { get; }

        /// <example>France</example>
        [Required]
        public string Country { get; }

        public AddressRequest(string street, string city, int postCode, string country)
        {
            Street = street;
            City = city;
            PostCode = postCode;
            Country = country;
        }
    }

    internal RegisterCommand ToCommand() =>
        new(
            FirstName: FirstName,
            LastName: LastName,
            Birthdate: Birthdate,
            Email: Email,
            Password: Password,
            Address: new(
                Street: Address.Street,
                City: Address.City,
                PostCode: Address.PostCode,
                Country: Address.Country));
}
