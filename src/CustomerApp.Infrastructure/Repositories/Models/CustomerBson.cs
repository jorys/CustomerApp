using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CustomerApp.Domain.Aggregates.Customers;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class CustomerBson
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    internal Guid CustomerId { get; init; }

    [BsonElement("firstName")]
    internal string FirstName { get; init; }

    [BsonElement("lastName")]
    internal string LastName { get; init; }

    [BsonElement("birthdate")]
    internal DateOnly Birthdate { get; init; }

    [BsonElement("email")]
    internal string Email { get; init; }

    [BsonElement("password")]
    internal string HashedPassword { get; init; }

    [BsonElement("status")]
    internal string CustomerStatus { get; init; }

    [BsonElement("address")]
    internal AddressBson Address { get; init; }

    public CustomerBson(Guid customerId, string firstName, string lastName, DateOnly birthdate, string email, string hashedPassword, string customerStatus, AddressBson address)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        Email = email;
        HashedPassword = hashedPassword;
        CustomerStatus = customerStatus;
        Address = address;
    }

    internal static CustomerBson From(Customer customer) =>
        new(
            customerId: customer.Id.Value,
            firstName: customer.FirstName.Value,
            lastName: customer.LastName.Value,
            birthdate: customer.Birthdate.Value,
            email: customer.Email.Value,
            hashedPassword: customer.HashedPassword.Value,
            customerStatus: customer.Status.Value,
            address: AddressBson.From(customer.Address));

    internal Customer ToDomain() =>
        Customer.ReloadFromRepository(
            customerId: CustomerId,
            firstName: FirstName,
            lastName: LastName,
            birthdate: Birthdate,
            email: Email,
            hashedPassword: HashedPassword,
            status: CustomerStatus,
            street: Address.Street,
            city: Address.City,
            postCode: Address.PostCode,
            country: Address.Country);
}
