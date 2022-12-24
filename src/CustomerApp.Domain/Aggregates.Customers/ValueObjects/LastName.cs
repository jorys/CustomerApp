using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class LastName : ValueObject
{
    public string Value { get; }

    LastName(string value)
    {
        Value = value;
    }

    public static ErrorOr<LastName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.IsRequired(nameof(LastName));
        }
        return new LastName(value.ToTitleCase());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
