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

    internal static ErrorOr<FirstName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.IsRequired(nameof(FirstName));
        }
        return new FirstName(value.ToTitleCase());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static FirstName ReloadFromRepository(string value) => new(value);
}
