using CustomerApp.Domain.Common;

namespace CustomerApp.Domain.Aggregates.LoginAttempts.ValueObjects;

public sealed class AttemptDate : ValueObject
{
    public DateTime Value { get; }

    AttemptDate(DateTime value)
    {
        Value = value;
    }

    internal static AttemptDate Create()
    {
        return new AttemptDate(DateTime.UtcNow);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static AttemptDate ReloadFromRepository(DateTime value) => new(value);
}
