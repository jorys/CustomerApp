using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class Password : ValueObject
{
    const int _minLength = 8;

    public string Value { get; }
    Password(string value)
    {
        Value = value;
    }

    public static ErrorOr<Password> Create(string value)
    {
        var errors = new List<Error>(4);
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(Errors.IsRequired(nameof(Password)));
        }
        if (value.Length < _minLength)
        {
            errors.Add(Errors.MinLength(nameof(Password), _minLength));
        }
        if (!value.Any(char.IsNumber))
        {
            errors.Add(Errors.MustContainNumber(nameof(Password)));
        }
        if (value.All(char.IsLetterOrDigit))
        {
            errors.Add(Errors.MustContainSpecialCharacter(nameof(Password)));
        }
        if (errors.Any()) return errors;

        return new Password(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
