using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class AddressBson
{
    [BsonElement("street")]
    internal string Street { get; }

    [BsonElement("city")]
    internal string City { get; }

    [BsonElement("postCode")]
    internal int PostCode { get; }

    [BsonElement("country")]
    internal string Country { get; }

    public AddressBson(string street, string city, int postCode, string country)
    {
        Street = street;
        City = city;
        PostCode = postCode;
        Country = country;
    }

    internal static AddressBson From(Address address) =>
        new(
            street: address.Street,
            city: address.City,
            postCode: address.PostCode,
            country: address.Country);
}
