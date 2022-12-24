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

    public static ErrorOr<HashedPassword> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.IsRequired(nameof(HashedPassword));
        }
        return new HashedPassword(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
