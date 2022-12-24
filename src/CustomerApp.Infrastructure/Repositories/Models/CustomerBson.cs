using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CustomerApp.Domain.Aggregates.Customers;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class CustomerBson
{
    [BsonId]
    [BsonElement("_id")]
    internal Guid CustomerId { get; }

    [BsonElement("firstName")]
    internal string FirstName { get; }

    [BsonElement("lastName")]
    internal string LastName { get; }

    [BsonElement("birthdate")]
    internal DateOnly Birthdate { get; }

    [BsonElement("email")]
    internal string Email { get; }

    [BsonElement("password")]
    internal string HashedPassword { get; }

    [BsonElement("status")]
    internal string CustomerStatus { get; }

    [BsonElement("address")]
    internal AddressBson Address { get; }

    internal CustomerBson(Guid customerId, string firstName, string lastName, DateOnly birthdate, string email, string hashedPassword, string customerStatus, AddressBson address)
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
}
