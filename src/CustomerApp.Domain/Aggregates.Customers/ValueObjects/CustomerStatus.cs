using CustomerApp.Domain.Common;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class CustomerStatus : ValueObject
{
    const string Active = "active";
    const string Locked = "locked";

    readonly static string[] _availableStatuses = new[] { Active, Locked };

    public string Value { get; }

    CustomerStatus(string value)
    {
        Value = value;
    }

    internal static CustomerStatus CreateActive() => new(Active);

    internal static CustomerStatus CreateLocked() => new(Locked);

    internal bool IsLocked() => Value == Locked;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static CustomerStatus ReloadFromRepository(string value) => new(value);
}
