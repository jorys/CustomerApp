using CustomerApp.Domain.Common;
using ErrorOr;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.Domain.Common.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    Email(string value)
    {
        Value = value;
    }

    public static ErrorOr<Email> Create(string value)
    {
        if (!new EmailAddressAttribute().IsValid(value))
        {
            return Errors.InvalidFormat(nameof(Email));
        }
        if (value.EndsWith("@yopmail.com"))
        {
            return Errors.InvalidTemporaryEmail(nameof(Email));
        }
        return new Email(value.ToLowerInvariant());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
