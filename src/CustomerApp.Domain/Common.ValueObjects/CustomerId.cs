using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.ValueObjects;

public sealed class CustomerId : ValueObject
{
    public Guid Value { get; }

    CustomerId(Guid value)
    {
        Value = value;
    }

    public static CustomerId Create()
    {
        return new CustomerId(Guid.NewGuid());
    }
    public static ErrorOr<CustomerId> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Errors.InvalidValue(nameof(CustomerId));
        }
        return new CustomerId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
