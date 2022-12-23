﻿using CustomerApp.Domain.Common;
using ErrorOr;
using System.Runtime.InteropServices;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class Address : ValueObject
{
    private const short _postCodeLength = 5;
    private static readonly string[] _authorizedCountries = new[] { "france" };

    public string Street { get; }
    public string City { get; }
    public int PostCode { get; }
    public string Country { get; }

    Address(string street, string city, int postCode, string country)
    {
        Street = street;
        City = city;
        PostCode = postCode;
        Country = country;
    }

    public static ErrorOr<Address> Create(string street, string city, int postCode, string country)
    {
        var errors = new List<Error>(4);
        if (string.IsNullOrWhiteSpace(street))
        {
            errors.Add(Errors.IsRequired(nameof(Street)));
        }
        if (string.IsNullOrWhiteSpace(city))
        {
            errors.Add(Errors.IsRequired(nameof(City)));
        }
        if (postCode.ToString().Length != _postCodeLength)
        {
            errors.Add(Errors.InvalidLength(nameof(PostCode), _postCodeLength));
        }
        var formattedCountry = country.FormatAsTitle();
        if (!_authorizedCountries.Contains(formattedCountry))
        {
            errors.Add(Errors.InvalidValue(nameof(Country), _authorizedCountries));
        }
        if (errors.Any()) return errors;

        return new Address(
            street: street,
            city: city,
            postCode: postCode,
            country: formattedCountry);
    }

    public ErrorOr<Address> With(string? street, string? city, int? postCode, string? country)
    {
        return Create(
            street: street ?? Street,
            city: city ?? City,
            postCode: postCode ?? PostCode,
            country: country ?? Country);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return PostCode;
        yield return Country;
    }
}
