using CustomerApp.Domain.Common;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class CustomerStatus : ValueObject
{
    const string Active = "active";
    const string Locked = "locked";

    public string Value { get; }

    CustomerStatus(string value)
    {
        Value = value;
    }

    public static CustomerStatus CreateActive()
    {
        return new CustomerStatus(Active);
    }

    public static CustomerStatus CreateLocked()
    {
        return new CustomerStatus(Locked);
    }

    public bool IsLocked() => Value == Locked;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
