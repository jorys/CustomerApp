using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class FirstName : ValueObject
{
    public string Value { get; }

    FirstName(string value)
    {
        Value = value;
    }

    public static ErrorOr<FirstName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.IsRequired(nameof(FirstName));
        }
        return new FirstName(value.FormatAsTitle());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
