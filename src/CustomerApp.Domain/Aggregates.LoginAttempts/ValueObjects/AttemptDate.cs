using CustomerApp.Domain.Common;

namespace CustomerApp.Domain.Aggregates.LoginAttempts.ValueObjects;

public sealed class AttemptDate : ValueObject
{
    public DateTime Value { get; }

    AttemptDate(DateTime value)
    {
        Value = value;
    }

    public static AttemptDate Create()
    {
        return new AttemptDate(DateTime.UtcNow);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
