using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class HashedPassword : ValueObject
{
    public string Value { get; }
    HashedPassword(string value)
    {
        Value = value;
    }

    internal static ErrorOr<HashedPassword> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.IsRequired(nameof(HashedPassword));
        }
        return new HashedPassword(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static HashedPassword ReloadFromRepository(string value) => new(value);
}
